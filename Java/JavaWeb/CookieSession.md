# Cookie

Cookie是服务器通知客户端保存键值对的一种技术

客户端有了Cookie后，每次请求都会发送给服务器

每个Cookie大小不能超过4kb





## Cookie的创建流程

1.没有Cookie的客户端(浏览器)访问服务器

2.服务器创建Cookie`Cookie cookie = new Cookie("key1","value1");`

```java
//创建Cookie对象
Cookie cookie = new Cookie("key1","value1");
//通知客户端保存Cookie
resp.addCookie(cookie);
```

3.`resp.addCookie(cookie);`**通过响应头Set-Cookie（Set-Cookie:key1=value1）通知客户端保存Cookie，如果少了这句，在浏览器(客户端)上查看不到Cookie**

4.收到响应后，发现有Set-Cookie响应头，就去看一下有没有相同key的Cookie，没有就创建，有就修改



## 服务器获取客户端的Cookie流程

1.客户端(浏览器)有了Cookie后，再次访问服务器，会通过请求头：Cookie：xx=xx把Cookie信息发送给服务器

2.服务器获取客户端发送过来的Cookie`request.getCookies()//返回Cookie数组`

这里如果需要从数组中查找到指定的key的Cookie，可以写一个查找的工具类

```java
public class CookieUtils{
    public static Cookie findCookie(String name,Cookie[] cookies){
        //防止空指针异常
        if(name==null || cookies==null || cookies.length == 0){
            return null;
        }
        //查找
        for(Cookie cookie : cookies){
            if(name.equals(cookie.getName())){
                return cookie;
            }
        }
        return null;//没查到返回null
    }
}
```

在Servlet程序中使用

```java
Cookie[] cookies = req.getCookies(); 
Cookie cookie = CookieUtils.findCookie("keyname",cookies);
if(cookie != null){
    resp.getWriter.write("找到了需要的Cookie");
}
```



## Cookie值的修改

第一种

1.创建一个和要修改的Cookie相同key的Cookie对象

2.在构造器中赋予新的value值

3.调用responce.addCookie(cookie);



第二种

1.先查找的需要修改的Cookie对象

2.调用setValue()方法赋予新的Cookie值

3.调用responce.addCookie()通知客户端保存修改

```java
Cookie[] cookies = req.getCookies(); 
Cookie cookie = CookieUtils.findCookie("keyname",cookies);
if(cookie != null){
    cookie.setValue("newValue");
    resp.getWriter.write("找到了需要的Cookie");
}
resp.addCookie(cookie);
```



## Cookie的生命控制

指的是管理Cookie什么时候被删除

setMaxAge()   默认值为-1

- 正数，表示在指定的秒数后过期
- 负数，表示浏览器关闭，Cookie就会被删除
- 零，表示马上删除

要让客户端保存修改，依然要在Servlet程序中set后`resp.addCookie(cookie);`



## Cookie有效路径的设置

Cookie的path属性可以有效的过滤哪些Cookie可以发送给服务器,哪些不发。

path属性是通过请求的地址来进行有效的过滤。

假设有以下两个Cookie

CookieApath=/工程路径

CookieBpath=/工程路径/abc

请求地址如下：

`http://ip:port/工程路径/a.html`

CookieA发送CookieB不发送

`http://ip:port/工程路径/abc/a.html`

CookieA发送CookieB发送

```java
Cookie cookie=newCookie("path1","path1");

//getContextPath()===>>>>得到工程路径
cookie.setPath(req.getContextPath()+"/abc");//===>>>>/工程路径/abc

resp.addCookie(cookie);
resp.getWriter().write("创建了一个带有Path路径的Cookie");
```

如果要修改已有的Cookie的path,先获取指定Cookie，再setPath，最后`resp.addCookie(cookie);`。或者new一个相同key的Cookie，setPath



## 用Cookie来实现免用户名登录

第一次访问的登录界面，无Cookie，提交表单数据后，在Servlet程序验证用户名密码是否正确，如果正确就创建Cookie，把用户名作为value存到Cookie对象中，再通过`resp.addCookie(cookie);`发生请求头Set-Cookie:username=用户名，使Cookie保存在客户端。

第二次访问该登录界面时，会把Cookie信息发给服务器，登录页面获取到cookie，把用户名的value值设为cookie的valuee值





# Session

**保存在服务器内存中**

1.Session就一个接口（HttpSession）。

2.Session就是会话。它是用来维护一个客户端和服务器之间关联的一种技术。

3.每个客户端都有自己的一个Session会话。

4.Session会话中，我们经常用来保存用户登录之后的信息。



## 创建和获取Session

创建和获取都是`request.getSession()`，第一次调用是创建Session会话，之后都是获取前面创建号的Session会话对象

isNew()	判断是否是刚创建的Session对象

每个Session的Id是唯一的，getId()获取



## 生命周期控制

public void setMaxInactiveInterval(intinterval) 设置Session的超时时间（以秒为单位），超过指定的时长，Session就会被销毁。

值为正数的时候，设定Session的超时时长。

负数表示永不超时（极少使用）

public int getMaxInactiveInterval() 获取Session的超时时间

public void invalidate()让当前Session会话马上超时无效。

Session默认的超时时间长为30分钟。因为在Tomcat服务器的配置文件web.xml中默认有以下的配置，它就表示配置了当前Tomcat服务器下所有的Session超时配置默认时长为：30分钟。

```xml
<session-config><session-timeout>30</session-timeout></session-config>
```

要修改个别的超时时长可以用`session.setMaxInactiveInterval(intinterval)`单独设置超时时长。，要更改所有的Session的可以在web.xml文件中修改。

超时时长指的是客户端两次请求的最大间隔时长



## Cookie和Session

Session底层其实是基于Cookie技术实现的

最初客户端没有Cookie信息，访问服务器后`request.getSession`创建会话对象，并保存在服务器内存中。而每次创建会话的，都会创建一个Cookie对象，这个Cookie对象的key永远是JESSIONID(新创建出来的session)，之后会通过响应把心创建出来的带有Session的id值返回给客户端，客户端解析收到的数据后就创建出一个cookie对象。

有了Cookie之后，每次请求都会把Session的id以Cookie的形式发给服务器，而服务器会通过Cookie的id值在服务器内存中找到创建号的Session对象，并返回。

