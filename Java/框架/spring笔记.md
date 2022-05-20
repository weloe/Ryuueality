Spring中的扫描

扫描查询有@Component等等注解的类

扫描到了之后需要进行解析，生成bean

怎么根据包名得到包下所有的类？真正得到的应该是Class对象

类加载器(解析的类的路径)：Bootstrap(jre/lib)、Ext(jre/ext/lib)、App(classpath)

[如何优雅的扫描指定包下的所有class - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/355050724)



```java
BeanNameAware//实例化之后，初始化之前
InitializingBean//初始化
BeanPostProcessor//实例化之前
```

Aware回调和初始化

BeanPostProcessor

提供给程序员使用的扩展机制

BeanPostProcessor提供给程序员在实例化对象之后，在实例化之前，回调之后初始化之前，初始化后能够做一些处理的机制

Class.isAssignableFrom isAssignableFrom 是用来**判断一个类Class1和另一个类Class2是否相同或是另一个类的超类或接口**。
