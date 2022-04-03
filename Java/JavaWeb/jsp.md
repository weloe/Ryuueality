# jsp

使用servlet程序往客户端输出页面较为繁琐

```java
//设置返回的数据内容的数据类型和编码
responcec.setContentType("text/html",charset=utf-8);
//获取字符输出流
Writer writer = responce.getWriter();
//输出页面内容
writer.write("<!DOCTYPE html>");
writer.write("<html>");
writer.write("<head>");
....

```

通过Servlet输出简单的html页面信息都非常不方便。那我们要输出一个复杂页面的时候，就更加的困难，而且不利于页面的维护和调试。所以sun公司推出一种叫做jsp的动态页面技术帮助我们实现对页面的输出繁锁工作。

1、jsp页面是一个类似于html的一个页面。jsp直接存放到WebContent目录下，和html一样访问jsp的时候，也和访问html一样。

2、jsp的默认编码集是iso-8859-1修改jsp的默认编码为UTF-8





jsp本质，其实是一个Servlet程序。

运行web工程，访问jsp文件的路径，Tomcat目录下的work\Catalina\localhost目录下的工程目录内会生成xxxx_jsp.java，xxxx_jsp.class。

xxxx_jsp.java里的类继承于HttpJspBase类，这是jsp文件生成Servlet程序要继承的基类，而HttpJspBase类继承于HttpServlet类，所以jsp其实也是一个Servlet程序

j也可以发现sp页面中的html内容被翻译到Servlet程序的service方法中输出，就是说jsp其实是可以用来输出html页面的Servlet程序





## 头文件声明

language属性 值只能是java。表示翻译的得到的是java语言

contentType属性 设置响应头contentType的内容

pageEncoding属性 设置当前jsp页面的编码

import属性 给当前jsp页面导入需要使用的类包

autoFlush属性 设置是否自动刷新out的缓冲区，默认为true

buffer属性 设置out的缓冲区大小。默认为8KB

errorPage属性 设置当前jsp发生错误后，需要跳转到哪个页面去显示错误信息

isErrorPage属性 设置当前jsp页面是否是错误页面。是的话，就可以使用exception异常对象

session属性 设置当前jsp页面是否获取session对象,默认为true

extends属性 给服务器厂商预留的jsp默认翻译的servlet继承于什么类



## 声明脚本

```jsp
<%!
	java代码
%>
```

1.我们可以定义全局变量。

2.定义static静态代码块

3.定义方法

4.定义内部类

几乎可以写在类的内部写的代码，都可以通过声明脚本来实现



## 表达式脚本

```
<%= 表达式 %>
```

用于向页面输出内容

翻译到Servlet程序的service方法中是用 out.print() 打印输出

out是jsp的一个内置对象，用于表达式生成html的源代码

**表达式不能以分号结尾**

可以输出任意类型



## 代码脚本

```
<% java代码 %>
```

代码脚本里可以写任意的Java语句，都会被翻译到service方法中，所有service方法中可以写的Java代码都可以写到代码脚本中



## 注释

```
//单行Java注释
/*
	多行Java代码注释
*/
```

单行注释和多行注释能在防疫后的Java源代码中看见

```
<%-- jsp注释 --%>
```

jsp注释在防御的时候会被忽略

```
<!-- html注释 -->
```

html的注释会被翻译到Java代码中输出到html页面中查看



## 九大内置对象(可以在代码脚本和表达式脚本直接使用的对象)

request对象 请求对象，可以获取请求信息

response对象 响应对象。可以设置响应信息

pageContext对象 当前页面上下文对象。可以在当前上下文保存属性信息

session对象 会话对象。可以获取会话信息。

exception对象 异常对象只有在jsp页面的page指令中设置isErrorPage="true"的时候才会存在

application对象 ServletContext对象实例，可以获取整个工程的一些信息。

config对象 ServletConfig对象实例，可以获取Servlet的配置信息

out对象 输出流。

page对象 表示当前Servlet对象实例（无用，用它不如使用this对象）。



## 四大域对象

pageContext可以保存数据在同一个jsp页面中使用

request可以保存数据在同一个request对象中使用。经常用于在转发的时候传递数据

session可以保存在一个会话中使用

application(ServletContext)就是ServletContext对象 保存于整个工程进程中



## out和responce的writer的区别

所有jsp中的out输出流的内容都必须要先flush写入到Responce的writer对象的缓冲区中，才能最终输出到客户端(浏览器)

jsp的out输出到Responce的Writer对象的缓冲区，永远是追加到writer缓冲区的末尾



## 常用标签

如果有许多个页面，每个页面都有一个共同的部分，那么就需要使用到包含

静态包含`<%@includefile=""%>`静态包含是把包含的页面内容原封装不动的输出到包含的位置。

动态包含`<jsp:includepage=""></jsp:include`>动态包含会把包含的jsp页面单独翻译成servlet文件，然后在执行到时候再调用翻译的servlet程序。并把计算的结果返回。动态包含是在执行的时候，才会加载。所以叫动态包含。

页面转发`<jsp:forwardpage=""></jsp:forward><jsp:forward`转发功能相当于request。`getRequestDispatcher("/xxxx.jsp").forward(request,responce);`的功能



## Listener



ServletContextListener

定义一个类，继承上面周期的监听器接口

在Web.xml文件中配置

contextInitialized——对象的创建回调，启动工程时调用方法

contextDestroyed——对象的销毁回调，停止工程时调用



# EL表达式

代替jsp夜猫子的表达式脚本在jsp页面中进行数据的输出

EL表达式的格式是：${表达式}EL表达式在输出null值的时候，输出的是空串。jsp表达式脚本输出null值的时候，输出的是null字符串。



EL表达式主要是在jsp页面中输出数据，主要是输出域对象的数据

当四个域中都有相同的key的数据的时候，EL表达式会按照四个域的小到大范围顺序去搜索，找到就输出



EL表达式输出Bean的属性，实质上是调用getXXX方法



## 运算

关系运算，逻辑运算，算数运算，empty运算

empty运算可以判断一个数据是否为空，如果为空，则输出true,不为空输出false。

1、值为null值的时候，为空

2、值为空串的时候，为空

3、值是Object类型数组，长度为零的时候

4、list集合，元素个数为零5、map集合，元素个数为零

三元运算`表达式1?表达式2:表达式3`



"."点运算和[]中括号运算符

.点运算，可以输出Bean对象中某个属性的值。

[]中括号运算，可以输出有序集合中某个元素的值。并且[]中括号运算，还可以输出map集合中key里含有特殊字符的key的值。例如

```jsp
<body>
<%
    Map<String,Object>map=newHashMap<String,Object>();
    map.put("a.a.a","aaaValue");
    map.put("b+b+b","bbbValue");
    map.put("c-c-c","cccValue");
    request.setAttribute("map",map);
%>
    ${map['a.a.a']}<br>
    ${map["b+b+b"]}<br>
    ${map['c-c-c']}<br>
</body>
```



## 11个隐含对象

pageContext

pageScope

requestScope

sessionScope

applicationScope

param

paramValues

header

headerValues

cookie

### scope获取特定域中的属性

pageScope======pageContext域

requestScope======Request域

sessionScope======Session域

applicationScope======ServletContext域

```jsp
<body>
	<%
		pageContext.setAttribute("key1","pageContext1");
		pageContext.setAttribute("key2","pageContext2");
		request.setAttribute("key2","request");
		session.setAttribute("key2","session");
		application.setAttribute("key2","application");
	%>
    ${applicationScope.key2}
</body>
```

### pageContext对象

request.getScheme()它可以获取请求的协议

request.getServerName()获取请求的服务器ip或域名

request.getServerPort()获取请求的服务器端口号

request.getContextPath()获取当前工程路径

request.getMethod()获取请求的方式（GET或POST）

request.getRemoteHost()获取客户端的ip地址

session.getId()获取会话的唯一标识

### param,paramValues

获取请求参数

### header,headerValues

获取请求头的信息

### cookie

获取当前请求的cookie信息

### initParam

它可以获取在web.xml中配置的<context-param>上下文参数



# JSTL标签库

jsp标准标签库

EL表达式主要是为了替换jsp中的表达式脚本，而标签库则是为了替换代码脚本。
