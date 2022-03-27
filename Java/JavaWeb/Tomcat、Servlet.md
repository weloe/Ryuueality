Tomcat：由Apache组织提供的一种Web服务器，提供对jsp和Servlet的支持。它是一种轻量级的javaWeb容器（服务

器），也是当前应用最广的JavaWeb服务器（免费）。

# Servlet

1、Servlet是JavaEE规范之一。规范就是接口

2、Servlet是JavaWeb三大组件之一。三大组件分别是：Servlet程序、Filter过滤器、Listener监听器。

3、Servlet是运行在服务器上的一个java程序，它可以接收客户端发送过来的请求，并响应数据给客户端。



实现servlet程序

- 编写一个类实现Servlet接口
- 实现service方法，处理请求，响应数据
- 在web.xml中配置servlet程序的访问地址





Servle生命周期

1、执行Servlet构造器方法，是在第一次访问的时候创建Servlet程序会调用。

2、执行init初始化方法，是在第一次访问的时候创建Servlet程序会调用。

3、执行service方法，每次访问都会调用。

4、执行destroy销毁方法，在web工程停止的时候调用。

请求分为两种GET和POST，需要分别处理，而HttpServlet中用doGe方法和doPost方法分别处理两种请求方式。所以一般Servlet程序都是继承HttpServlet，然后根据业务需要重写doGet或doPost方法





ServletConfig类

Servlet程序的配置信息类。Servlet程序和ServletConfig对象都是由Tomcat负责创建，我们负责使用。

Servlet程序默认是第一次访问的时候创建，ServletConfig是每个Servlet程序创建时，就创建一个对应的ServletConfig对象。

作用：





