ThreaLocal的使用

jdk中用于解决多线程的数据安全的工具类

提供了线程局部变量， 每个线程都有自己的局部变量，但它独立于变量的初始化副本，ThreadLocal实例是类中的private static字段。

每个线程都保持对其线程局部变量副本的隐式引用，只要线程是活动的并且是ThreadLocal实例是可访问的，在线程消失后，其线程局部实例的所有副本都会被垃圾回收

ThreadLocal可用给当前线程关联一个数据(可以是普通变量，可以是对象，也可以是数组，集合)——可以避免其他线程访问到这个线程的数据

特点：

1、ThreadLocal可以为当前线程关联一个数据(类似Map一样存取数据，key为当前线程)



既然如此为何不用HashMap或者ConcurrentHashMap来实现类似的效果？？



2、**一个ThreadLocal对象实例只能为当前线程关联一个数据**，如果要为当前线程关联多个数据，需要使用多个ThreadLocal对象实例

如果一个ThreadLocal对象实例 set了两个数据，取出的是 后set 的数据



3、每个ThreadLocal对象定义的时候，一般都是static类型

4、ThreadLocal保存的数据，在线程销毁后会由JVM自动释放

只要是当前线程关联的数据不管代码层级调用有多深，不妨碍数据的获取





使用ThreadLocal确保所有数据库操作使用同一个Connection连接对象

前提：在同一个线程(ThreadLocal是与当前线程关联)

```java
ThreadLocal<Connection> threadLocal = new ThreadLocal<>();
Connection conn = JdbcUtils.getConnection();
threadLocal.set(conn);

```

