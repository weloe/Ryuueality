# IOC

IOC(接口)

1.IOC思想基于IOC容器完成，IOC容器底层就是对象工厂

2.Spring提供IOC容器实现两种方式:(两个接口)

- BeanFactory：IOC容器基本实现，是Spring内部的使用接口，不提供开发人员使用。**加载配置文件时不会创建对象，在获取对象(使用)才去创建对象**
- ApplicationContext:BeanFactory接口的子接口，提供更多更强大的功能，一般由开发人员进行使用。**加载配置文件时就会把在配置文件对象进行创建**

## Bean管理(xml方式)

### 1.创建对象 

spring配置文件中，使用bean标签，标签里添加对应属性，就可以实现对象创建

bean标签有很多属性

id 对象别名，唯一标识，ApplicationContext.getBean(id,xx.class)

class 要创建的对象的全路径

name 作用和id一样，id不能加特殊符号，name可以

对象创建时执行默认的无参构造

### 2.属性注入

DI:依赖注入，注入属性

第一种:使用set方法注入 

创建类，定义属性和对应的set方法

在spring配置文件配置对象创建， 再配置属性注入

name: 类的属性的名称 	value：向属性注入的值

```xml
<bean id="" class="" >
	<property name="bname" value="123"></property>
    <property name="bauthor" value="v789"></property>
</bean>
```

简化的写法--p名称空间注入

1.使用p名称空间注入，简化基于xml配置方式

在xml约束中加个属性

```xml
xmlns:p="http://www.springframework.org/schema/p"
```

```xml
<bean id="" class="" p:bname="456" p:bauthor="789">
</bean>
```

第二种：使用有参构造进行注入

spring配置文件中进行配置

name是有参构造的参数名 value是要设置的值

```xml
<bean id="" class="">
    <constructor-arg name="" value=""></constructor-arg>	
    <constructor-arg name="" value=""></constructor-arg>
</bean>
```

#### 2.1 IOC操作Bean管理(xml注入其他类型属性)

1.字面量

设置属性为null值

```xml

<property name="bname">
	<null/>
</property>
```

属性值直接包含包含特殊符号会出错

```xml
<property name="address" value="<<sds>>"></property>	<!--error-->
```

把<做转义`&lt;`

或者写在CDATA中

```xml
<property name="address">
	<value><![CDATA[<<sds>>]]></value>
</property>
```

#### 2.2注入属性-外部Bean

创建两个类service类和dao类，在service调用dao里面的方法

#### 2.3注入属性-内部Bean和级联赋值

#### 2.4注入集合类型属性

#### 2.5提取集合注入部分

在spring配置文件中引入名称空间util

使用util标签完成list集合注入提取

```xml
<!--   提取list集合类型属性注入  -->
    <util:list id="bookList">
        <value>yjj</value>
        <value>jyzj</value>
        <value>jysg</value>
    </util:list>

<!--    提取list集合类型属性注入使用    -->
    <bean id="book" class="com.example.springstudy.Book">
        <property name="list" ref="bookList"></property>
    </bean>
```

### 3.FactoryBean

Spring有两种bean，一种普通bean，另外一种工厂bean(FactoryBean)

普通bean：在配置文件中定义的bean是什么类型就返回什么类型，

工厂bean：在配置文件定义的bean类型可用和返回类型不一样

### 4.bean作用域

在Spring里面，设置创建bean实例是单实例还是多实例

Spring默认情况下创建的bean是单实例对象

bean标签的scope属性可用于设置单实例还是多实例

第一个值 默认值，singleton，表示实单实例对象

第二个值 prototype，表示多实例对象

singleton和prototype区别

第一singleton单实例，prototype多实例

第二 设置scope值是singleton时候，加载spring配置文件时就会创建单实例对象

设置成 prototype时候，不是在加载spring配置文件时创建对象，在调用getBean方法时候创建多实例对象

request	创建时存到request域中					

session	创建时存到session域中 

### 5.bean生命周期

1.通过构造器创建bean实例(无参数构造)

2.为bean的属性设置值和对其他bean的引用(调用set方法)

把bean实例传递给bean后置处理器的方法postProcessBeforeIntialization

3.调用bean的初始化的方法(需要进行配置)

把bean实例传递bean后置处理器方法postProcessAfterIntialization

4.bean可以使用(对象获取到了)

5.当容器关闭时候，调用bean的销毁的方法(需要配置销毁的方法)

创建后置处理器，实现接口BeanPostProcessor

### 6.自动装配

根据指定的装配规则(属性名称或者属性类型)，Spring自动将匹配的属性值进行注入

bean标签属性autowire，配置自动装配	

autowire 属性值 

byName根据属性名称注入  注入的bean的id值和类属性名称要一致

byType根据属性类型注入 

### 7.引入外部属性文件



```
prop.driverClass=com.mysql.jdbc.Driver
prop.url=jdbc:mysql://localhost:3306/userDb
prop.userName=root
prop.password=root
```



```xml
<context:property-placeholder location="jdbc.properties"/>
```

```xml
<bean id="druidDataSource" class="com.alibaba.druid.pool.DruidDataSource">
        <property name="driverClassName" value="${prop.driverClass"></property>
        <property name="url" value="${prop.url}"></property>
        <property name="username" value="${prop.userName}"></property>
        <property name="password" value="${prop.password}"></property>
    </bean>
```

## 注解方式

开启组件扫描

context

```xml
    <context:component-scan base-package="com.example.springstudy.dao,com.example.springstudy.service"></context:component-scan>

```

# AOP

AOP底层使用了动态代理

有两种情况动态代理

第一种，有接口，使用JDK动态代理

创建接口实现类代理对象

第二种，没有接口情况，使用CGLIB动态代理

连接点:类里面哪些方法可以被增强，这些方法称为连接点

切入点:实际被真正增强的方法，称为切入点

通知(增强):实际增强的逻辑部分称为通知(增强)，分为前置通知，后置通知，环绕通知，异常通知，最终通知finally。

切面是动作：把通知应用到切入点的过程

AOP操作

Spring框架一般都是基于AspectJ实现AOP操作

AspecJ不是Spring组成部分，是独立的AOP框架，一般和Spring框架一起使用来实现AOP，需要springsource包

基于AspectJ思想AOP操作

基于xml配置文件方式实现，基于注解方式实现，一般用注解方式

切入点表达式：知道哪个类里面的哪个方法进行增强

语法结构：

`exeuction([权限修饰符][返回类型][方法名称]([参数列表]))`

```java
execution(* com.example.dao.BookDao.add(..))//*任意修饰符，返回类型可以省略
execution(* com.example.dao.BookDao.*(..))//对所有方法增强
execution(* com.example.dao.*.*(..))//对这个包里的所有方法增强
```

## AOP操作(AspectJ配置文件)

创建两个类，增强类和被增强类，创建方法

在spring配置文件中创建两个类对象

在spring配置文件中配置切入点







## AOP操作(AspectJ注解)

1.创建类，定义方法 

2.创建增强类

3.通知的配置：开启注解扫描,注解创建这两个对象，在增强类上面添加注解@Aspect，在spring配置文件中开启生成代理对象

4.配置不同类型的通知:在增强类里，在作为通知的方法上添加通知类型的注解，并且使用切入点表达式配置



相同的切入点抽取，重用切入点的定义

```java
	//相同切入点抽取
    @Pointcut(value = "execution(* com.example.aopano.User.add(..))")
    public void pointDemo(){
		
    }

	@Before(value = "pointDemo()")
    public void before() {
        System.out.println("before...");
    }

```

一个方法有多个增强类，可以设置增强类的优先级

 增强类上添加注解@Order(数字类型值)，数字类型值越小，优先级越高，数字小的after较后执行，before先执行

完全注解开发 配置类

```java
@Configuration
@ComponentScan(basePackages = {"com.example.aopano"})
@EnableAspectJAutoProxy(proxyTargetClass = true)
public class ConfigAop {

}
```

# JdbcTemplate

导入jar包

spring配置文件中配置数据库连接池

配置JdbcTemplate，注入属性DataSource 

配置文件中 开启组件扫描，service：创建service，注入dao，dao：创建dao，注入JdbcTemplate对象

jdbcTemplate.update(String sql,Object...args)

RowMapper 接口，返回不同类型的数据，使用这个接口的实现类完成数据的封装

# Spring事务管理

1.事务添加到JavaEE三层结构里面Service层(业务逻辑层)

2.在Spring进行事务管理操作

编程式事务管理，声明式事务管理(使用)

3.声明式事务管理

基于注解方式，基于xml配置文件方式

4.在Spring进行声明式的事务管理，底层使用**AOP**原理

5.Spring事务管理API

提供了一个接口，代表事务管理器，这个接口针对不同的框架提供不同的实现类。

## 注解方式

创建事务管理器，注入数据源，开启事务注解，service类上面(或者service类的方法上面)添加事务注解

加到类上表示所有方法都添加事务，方法上就只为这个方法加事务

注解也可以配置事务相关属性

声明式事务管理参数配置

propagation:事务传播行为 事务方法：对数据库表数据进行变化的操作 ，多事务方法调用中产生的行为

例如 add()方法调用update() 方法

REQUIRED 

如果add方法本身有事务，调用update方法之后，update使用当前add方法里面事务

如果add方法本身没有事务，调用update方法之后，创建新事务

REQUIRED_NEW 

使用add方法调用update方法，如果add无论是否有事务，都创建新的事务



isolation:事务隔离级别 事务有特性称为隔离性，多事务操作直接不会产生影响。不考虑隔离性会有脏读、不可重复度、虚读问题 

脏读：一个未提交的事务读取到了另一个未提交事务的数据



timeout:超时时间

readOnly:是否只读

rollbackFor 回滚

noRollbackFor 不回滚

```java


```

## xml方式

在spring配置文件中进行配置

配置事务管理器、配置通知、配置切入点、配置切面

## 完全注解

1.创建配置类，使用配置类替代xml配置文件

```java
@Configuration//配置类
@ComponentScan(basePackages = "com.example.springstudy.jdbcT")//组件扫描
@EnableTransactionManagement//开启事务
public class TxConfig {

    //创建数据库连接池
    @Bean
    public DruidDataSource getDruidDataSource() {
        DruidDataSource druidDataSource = new DruidDataSource();
        druidDataSource.setDriverClassName("");
        druidDataSource.setUrl("");
        druidDataSource.setUsername("");
        druidDataSource.setPassword("");
        return druidDataSource;
    }

    //创建JdbcTemplate
    @Bean
    public JdbcTemplate getJdbcTemplate(DataSource dataSource) {
        //到ioc容器中根据类型找到dataSource
        JdbcTemplate jdbcTemplate = new JdbcTemplate();

        //注入DataSource
        jdbcTemplate.setDataSource(dataSource);
        return jdbcTemplate;
    }

    //创建事务管理器对象
    @Bean
    public DataSourceTransactionManager getDataSourceTransactionManager(DataSource dataSource){
        DataSourceTransactionManager dataSourceTransactionManager = new DataSourceTransactionManager();
        dataSourceTransactionManager.setDataSource(dataSource);

        return dataSourceTransactionManager;
    }
```

# Spring新特性 

Spring5框架的代码基于Java8实现，允许兼容jdk9，许多不简易使用的类和方法在代码中删除

Spring5框架自带了通用的日志框架

Spring5移除了Log4jConfigListener，官方简易使用Log4j2

Spring5框架整合Log4j2

引入jar包，创建Log4j.xml配置文件

### 支持@Nullable注解

1.可以使用在方法，属性，参数上，表示方法返回值可以为空，属性值可以为空

核心容器支持函数式风格GenericApplicationContext	

```java
    @Test
    public void testGenericApplicationContext(){
        //创建  对象
        GenericApplicationContext genericApplicationContext = new GenericApplicationContext();
        //调用方法对象注册
        genericApplicationContext.refresh();
        //genericApplicationContext.registerBean(User.class,()->new User());
        genericApplicationContext.registerBean("user1",User.class,()->new User());
        //获取在spring注册的对象
        //User user = (User) genericApplicationContext.getBean("com.example.springstudy.User");
        User user = (User) genericApplicationContext.getBean("user1");
        System.out.println(user);

    }
```

### Spring5支持整合Junit5

整合Junit4

引入Spring针对测试的依赖，创建测试类

```java
@RunWith(SpringJUnit4ClassRunner.class)//单元测试框架
@ContextConfiguration("classpath:bean4.xml")//加载配置文件//相当于 ApplicationContext context = new AnnotationConfigApplicationContext("bean4.xml");

public class JTest4 {


}
```

整合Junit5

引入junit5 jar包 

创建测试类，使用注解完成

```java
@ExtendWith(SpringExtension.class)
@ContextConfiguration("classpath:bean2.xml")
public class JTest5 {

    @Test
    
}
```

`@ExtendWith(SpringExtension.class)
@ContextConfiguration("classpath:bean2.xml")`可以用复合注解替代`@SpringJUnitConfig(locations = "classpath:bean2.xml")`

### SpringWebFlux

异步非阻塞，不扩充硬件资源的前提下提升系统的吞吐量和伸缩型

非阻塞式

函数式：基于Java8，Webflux使用Java8函数式编程方式实现路由请求 

springMVC命令式

webflux响应式

