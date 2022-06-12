之前做的项目中，controller依赖于service，service依赖于dao

下层改动会影响上层，存在耦合性（即层与层之间存在依赖)

而controller创建了service对象，service中创建了dao对象

只需要把对象的创建过程分离出来，就能降低耦合度(减小上层对下层的依赖)

可以利用xml文件创建对象

IOC(控制反转)/DI(依赖注入)

控制反转:

1.之前在servlet中创建service对象是用new的方法,如果在servlet的某一个方法内部new,这个service的作用域（生命周期）是这个方法级别，如果是在servlet类中作为成员变量，那么就是这个servlet实例级别

2.之后在applicationContext.xml中定义了这个service，通过解析xml，产生service实例，存放在beanMap中，这个BeanMap在一个BeanFactory中，也因此service不在servlet中，改变了之前的service实例、dao实例的生命周期。也就意味着控制器从程序员转移到BeanFactory。这个现象称之为控制反转

依赖注入:

1.之前在控制层new Service对象，控制层和service层存在耦合，将代码修改成`UserService userService = null;`

然后xml配置文件中配置，再通过解析xml文件，再通过反射找到service、dao实例，注入到controller和service的service、dao中


