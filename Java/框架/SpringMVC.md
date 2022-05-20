SpringMVC

将浏览器发起的请求统一交给前端控制器处理

## 配置web.xml

1.默认配置方式

SpringMVC配置文件的位置和名称，位置默认位于WEB-INF下，默认名为`<servlet-name>`-servlet.xml

```xml
<!--    配置springMVC的前端控制器，对浏览器发送的请求进行统一处理   -->
    <servlet>
        <servlet-name>springMVC</servlet-name>
        <servlet-class>org.springframework.web.servlet.DispatcherServlet</servlet-class>
    </servlet>
    <servlet-mapping>
        <servlet-name>springMVC</servlet-name>
<!--    -->
        <url-pattern>/</url-pattern>
    </servlet-mapping>
```

2.扩展配置方式

 

```xml
<!--    配置springMVC的前端控制器，对浏览器发送的请求进行统一处理   -->
    <servlet>
        <servlet-name>springMVC</servlet-name>
        <servlet-class>org.springframework.web.servlet.DispatcherServlet</servlet-class>
        <!--        配置springMVC配置文件的位置和名称-->
        <init-param>
            <param-name>contextConfigLocation</param-name>
            <param-value>classpath:springMVC.xml</param-value>
        </init-param>
        <!--        将前端控制器DispatcherServlet的初始化时间提前到服务器启动时-->
        <load-on-startup>1</load-on-startup>
    </servlet>
    <servlet-mapping>
        <servlet-name>springMVC</servlet-name>
        <!--  上下文路径下所有的请求路径  -->
        <url-pattern>/</url-pattern>
    </servlet-mapping>
```



```xml
<load-on-startup>1</load-on-startup>//将前端控制器DispatcherServlet的初始化时间提前到服务器启动时
```



DispatcherServlet 配置路径  /

所匹配的请求可以是/login或者.html或.js或.css方式的请求路径，但是不能匹配.jsp请求路径的请求

jsp本质就是servlet，访问jsp需要通过当前服务器特殊的servlet进行处理。jsp不需要DispatcherServlet处理。如果匹配了，就会被当成普通的请求进行处理，就无法找到相对的jsp页面

/*  能匹配所有请求，包括.jsp 



要让请求匹配到请求映射，就需要在控制器方法上加上`@RequestMapping(value="请求地址")` 控制方法返回视图名称，通过springMVC里配置的视图解析器加上前缀和后缀，进行访问

thymeleaf解决浏览器解析的绝对路径， 检测到使用绝对路径时会自动添加上下文路径

```html
<a th:href="@{/target}">访问目标页面target.html</a>
```

总结：浏览器发送请求，如果请求地址符合前端控制器的url-pattern，该请求就会被前端控制器DispatcherServlet处理。前端控制器会读取SpringMVC的核心配置文件，通过扫描组件找到控制器，将请求地址和控制器中@RequestMapping注解的value属性值进行陪陪，若匹配成功，该主机所标识的控制器方法就是处理该请求的方法。处理请求的方法需要返回一个字符串类型的视图名称，该视图名称会被视图解析器解析，加上前缀和后缀组成视图的路径，通过Thymeleaf对视图进行渲染，最终转发到视图所对应页面	

## RequestMapping

不能有两个value相同的@RequestMapping

# 共享数据

## 向request域中共享数据



### ServletAPI



### ModelAndView



### Model,ModelMap,Map

类名都为BindingAwareModelMap

 说明通过同一个类进行实例化  

ModelMap继承LinkedHashMap，所以ModelMap其实就是Map接口的实现类



### 

不管用哪种方法最后都会被封装到ModelAndView中

## 向session域中共享数据

### @SessionAttribute 

作用  把数据共享到请求域时，再把数据共享到Session

### ServletAPI



## 向application(ServletContext)域中共享数据

# 视图

## ThymeleaView

## 转发视图

## 重定向视图

## 视图控制器view-controller

当控制器方法中只用来实现页面跳转`return "xxx";`即只需呀设置视图名称时，可以将处理器方法配置在mvc配置文件中

```xml
<mvc:view-controller path="/" view-name="index"></mvc:view-controller>

```

在这配置后，控制器的请求映射全部失效，需要开启mvc的注解驱动

```xml
<mvc:annotation-driven/>
```

# RESTFul

表现层(视图和控制层)资源状态(当前资源的表现形式)转移(浏览器和服务器交互的一种状态/方式)

浏览器向服务器请求资源有各种方式，应该一个事务操作同一个资源，请求路径页应该一样，通过转移和操作浏览器请求路径去请求服务器资源，来间接实现操作资源的目的

相同的请求路径，不同的请求方式(GET,POST,PUT,DELETE)来标识对同一资源的不同操作

统一的请求路径

不再用加?key=name的形式进行传参，而是把所有的数据用/拼接到请求地址中

method不能直接设置为PUT和DELETE，如果设置则被默认为GET请求

可以通过通过框架的 HiddenHttpMethodFilter 把请求的method设置成_method参数的值来实现设置put和delete

web.xml配置文件

```xml
<!--  配置HiddenHttpMethodFilter-->
    <filter>
        <filter-name>HiddenHttpMethodFilter</filter-name>
        <filter-class>org.springframework.web.filter.HiddenHttpMethodFilter</filter-class>
    </filter>
    <filter-mapping>
        <filter-name>HiddenHttpMethodFilter</filter-name>
        <url-pattern>/*</url-pattern>
    </filter-mapping>
```

**设置编码的过滤器需要放在获得参数之前，否则会失效，而HiddenHttpMethodFilter执行的方法中获取了_method参数的值，因此如果要配置设置编码的过滤器要放在前面**

RESTFul修改功能

```html
<form th:action="@{/user}" method="post">
    <input type="hidden" name="_method" value="PUT">
    用户名：<input type="text" name="username"><br>
    密码：<input type="password" name="password"><br>
    <input type="submit" name="修改"><br>
</form>
```

```java
@RequestMapping(value = "/user", method = RequestMethod.PUT)
    public String updateUser(String username,String password){
        System.out.println("修改用户信息"+username+","+password);
        return "success";
    }
```



# 拦截器

## 拦截器配置

springMVC配置文件中配置

```xml
<!--    配置拦截器-->
    <mvc:interceptors>
<!--    <bean class="com.atguigu.mvc.interceptor.FirstInterceptor"></bean>-->
<!--        <ref bean="firstInterceptor"></ref>-->
        <mvc:interceptor>
            <mvc:mapping path="/*"/>
            <mvc:exclude-mapping path="/"/>
            <ref bean="firstInterceptor"></ref>
        </mvc:interceptor>
    </mvc:interceptors>
```

这的`/*`只表示工程路径下的一层目录，不是所有路径，所有路径应该是`/**`
