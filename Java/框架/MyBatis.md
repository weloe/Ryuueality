核心配置文件



创建Mapper接口，不需要实现类，调用接口中的方法时，mybatis会自动匹配sql语句并执行

核心配置文件可以设置mapper映射文件的resultType的别名，且不区分大小写,如果不设置alias就是类名且不区分大小写

```xml
    <!--设置类型别名(不区分大小写) -->
    <typeAliases>
        <typeAlias type="com.atguigu.mybatis.pojo.User" alias="User"></typeAlias>
    </typeAliases>
```

```xml
    <select id="方法名" resultType="User">
        sql语句
    </select>
```

```xml
<package name="com.atguigu.mybatis.pojo"/><!--以包为单位，将包下的所有类型设置默认的别名，即类名且不区分大小写-->
```

也可以以包为单位引入映射文件

在resource下创建package，以'/'分隔，以'.'分割会把'.'作为文件名字

```xml
<!--    引入映射文件  -->
    <mappers>
        <!--<mapper resource="mappers/UserMapper.xml"/>-->
        <!--
            以包为单位引入映射文件
            要求：
            1.mapper接口所在的包和映射文件所在的包一致
            2.mapper接口要和映射文件的名字一致
        -->
        <package name="com.atguigu.mybatis.mapper"/>
    </mappers>
```

# MyBatis获取参数值的两种方式：${}和#{}

${}本质字符串拼接，#{}本质占位符赋值,(会自动加单引号)

mapper接口方法的参数为单个字面量类型

通过${}和#{}以任意的名称获取参数值，但是需要注意${}的单引号问题

## 1.单个参数直接使用

## 2.多个参数时

会把参数放在一个map集合中以两种方式存储，arg0,arg1...为键，参数为值 或者 param1,param2...为键，参数为值，通过#{}和${}以键的方式访问值，注意${}的单引号问题

```xml
    <select id="checkLogin" resultType="User">
        select * from t_user where username =#{arg0}  and password =#{arg1}
    </select>
```

## 3.mapper接口方法的参数有多个时

可以手动将这些参数放到一个map中进行存储，通过自己设定的键#{key}访问值

```java
    @Test
    public void testGetUserByMap() {
        SqlSession sqlSession = SqlSessionUtils.getSqlSession();
        ParameterMapper mapper = sqlSession.getMapper(ParameterMapper.class);
        Map<String,Object> map = new HashMap<>();
        map.put("username","admin");
        map.put("password","123456");
        User user = mapper.checkLoginByMap(map);
        System.out.println(user);
    }
```



```xml
    <select id="checkLoginByMap" resultType="User">
        select * from t_user where username =#{username}  and password =#{password}
    </select>
```

## 4.mapper接口方法参数是实体类型的参数

通过#{}和${}以属性的方式访问属性值即可，但是需要注意${}的单引号问题

```java
    @Test
    public void testInsertUser() {
        SqlSession sqlSession = SqlSessionUtils.getSqlSession();
        ParameterMapper mapper = sqlSession.getMapper(ParameterMapper.class);
        int user = mapper.insertUser(new User(null,"ls","123",23,"男", "123@q."));
        System.out.println(user);
    }
```



```xml
    <insert id="insertUser">
        insert into t_user values(null ,#{username},#{password},#{age},#{sex},#{email})
    </insert>
```

## 5.@Param

使用@Param来命名参数，就会将这些参数放入map集合，以@Param的值为键，以参数为值

```java
@Test
public void testCheckLoginByParam() {
    SqlSession sqlSession = SqlSessionUtils.getSqlSession();
    ParameterMapper mapper = sqlSession.getMapper(ParameterMapper.class);
    User user = mapper.checkLoginByParam("admin","123456");
    System.out.println(user);
}
```

```xml
<select id="checkLoginByParam" resultType="User">
    select * from t_user where username = #{username} and password = #{password}
</select>
```

# 比较特殊的sql语句

模糊查询 like

```xml
        select * from t_user where username like '%${username}%'
        select * from t_user where username like concat('%',#{username},'%')
        select * from t_user where username like "%"#{username}"%"
```

动态表名

```xml
    <select id="getUserByTableName" resultType="User">
        select * from ${tableName}
    </select>
```

当表中的字段名和实体类中的属性名无法对应，可以通过两种方法解决

1.mybatis全局配置设置

```xml
<settings>
    <setting name="mapUnderscoreToCamelCase" value="true"/>
</settings>
```

2.resultMap设置自定义映射关系

```xml
	<resultMap id="empResultMap" type="Emp">
        <id property="eid" column="eid"></id>
        <result property="empName" column="emp_name"></result>
        <result property="age" column="age"></result>
        <result property="sex" column="sex"></result>
        <result property="email" column="email"></result>
    </resultMap>
    <select id="getAllEmp" resultMap="empResultMap">
        select * from t_emp
    </select>
```

## 处理多对一的映射关系

1.级联属性赋值

```xml
	<resultMap id="empAndDeptResultMapOne" type="Emp">
        <id property="eid" column="eid"></id>
        <result property="empName" column="emp_name"></result>
        <result property="age" column="age"></result>
        <result property="sex" column="sex"></result>
        <result property="email" column="email"></result>
        <result property="dept.did" column="did"></result>
        <result property="dept.deptName" column="dept_name"></result>
    </resultMap>
    <select id="getEmpAndDept" resultMap="empAndDeptResultMapOne">
        select * from t_emp left join t_dept on t_emp.did=t_dept.did where t_emp.eid=#{eid}
    </select>
```

2.association标签处理多对一映射关系

property:需要处理多对一的映射关系的属性名

javaType:该属性对应的Java类型



```xml
<resultMap id="empAndDeptResultMapTwo" type="Emp">
    <id property="eid" column="eid"></id>
    <result property="empName" column="emp_name"></result>
    <result property="age" column="age"></result>
    <result property="sex" column="sex"></result>
    <result property="email" column="email"></result>
    <association property="dept" javaType="Dept">
        <id property="did" column="did"></id>
        <result property="deptName" column="dept_name"></result>
    </association>
</resultMap>
```

3.分步查询

```xml
    <association property="dept"
                 select="com.atguigu.mybatis.mapper.DeptMapper.getEmpAndDeptByStepTwo"
                 column="did"></association>
```

select:设置分步查询的sql的唯一标识(namespace.SQLId或者mapper接口的全类名.方法名)

colum:设置分步查询的条件

```xml
<resultMap id="getEmpAndDeptByStepResultMap" type="Emp">
    <id property="eid" column="eid"></id>
    <result property="empName" column="emp_name"></result>
    <result property="age" column="age"></result>
    <result property="sex" column="sex"></result>
    <result property="email" column="email"></result>
    <association property="dept"
                 select="com.atguigu.mybatis.mapper.DeptMapper.getEmpAndDeptByStepTwo"
                 column="did"></association>
</resultMap>
<select id="getEmpAndDeptByStepOne" resultMap="getEmpAndDeptByStepResultMap">
    select * from t_emp where eid = #{eid}
</select>
```

```xml
<mapper namespace="com.atguigu.mybatis.mapper.DeptMapper">

<!--    Dept getEmpAndDeptByStepTwo(@Param("did") Integer did);
-->
    <select id="getEmpAndDeptByStepTwo" resultType="Dept">
        select * from t_dept where did = #{did}
    </select>
    
</mapper>
```

分步查询的延迟加载，需要设置全局配置信息开启(默认不开启)

lazyLoadingEnabled：延迟加载的全局开关，开启时，所有关联对象都会延迟加载

aggressiveLazyLoding：当开启时，任何方法的调用都会加载该对象的所有属性。否则，每个属性会按需加载

association标签的属性fetchType能单独设置延迟加载(lazy)或立即加载(eager),当开启了全局的延迟加载之后，可通过此属性手动控制延迟加载的效果。如果没开启lazy和eager都是立即加载

## 处理一对多的映射关系

1.使用collection标签

collection用于处理一对多的映射关系

ofType表示该属性所对应的集合中存储的数据的类型

```xml
    <resultMap id="deptAndEmpResultMap" type="Dept">
        <id property="did" column="did"></id>
        <result property="deptName" column="dept_name"></result>
        <collection property="emps" ofType="Emp">
            <id property="eid" column="eid"></id>
            <result property="empName" column="emp_name"></result>
            <result property="age" column="age"></result>
            <result property="sex" column="sex"></result>
            <result property="email" column="email"></result>
        </collection>
    </resultMap>
<!--    Dept getDeptAndEmp(@Param("did") Integer did);-->
    <select id="getDeptAndEmp" resultMap="deptAndEmpResultMap">
        select * from t_dept left join t_emp on t_dept.did=t_emp.did where t_dept.did=#{did}
    </select>
```

2.分步查询

select:设置分步查询的sql的唯一标识(namespace.SQLId或者mapper接口的全类名.方法名)

colum:设置分步查询的条件

collection标签的属性fetchType能单独设置延迟加载(lazy)或立即加载(eager),当开启了全局的延迟加载之后，可通过此属性手动控制延迟加载的效果。如果没开启lazy和eager都是立即加载

```xml
    <resultMap id="deptAndEmpByStepResultMap" type="Dept">
        <id property="did" column="did"></id>
        <result property="deptName" column="dept_name"></result>
        <collection property="emps"
                    select="com.atguigu.mybatis.mapper.EmpMapper.getDeptAndEmpByStepTwo"
                    column="did"></collection>
    </resultMap>
    <select id="getDeptAndEmpByStepOne" resultMap="deptAndEmpByStepResultMap">
        select * from t_dept where did=#{did}
    </select>
```

```xml
    <select id="getDeptAndEmpByStepTwo" resultType="Emp">
        select * from t_emp where did=#{did}
    </select>
```

# 动态SQL

根据特定条件动态拼接SQL语句的功能，存在意义是为了解决拼凑SQL语句字符串的痛点问题。

## 多条件查询

使用**if**标签 根据标签中test属性所对应的表达式来决定标签中的内容是否需要拼接到sql中

```xml
    <select id="getEmpByConditionOne" resultType="Emp">
        select * from t_emp where 1=1
        <if test="empName !=null and empName != '' ">
            and emp_name=#{empName}
        </if>
        <if test="age !=null and age !='' ">
            and age = #{age}
        </if>
        <if test="sex !=null and sex !='' ">
            and age = #{age}
        </if>
        <if test="email !=null and email !='' ">
            and age = #{age}
        </if>
    </select>

```

### **where**标签

当where标签中有内容时，会自动生成where关键字，并自动将内容前多余的and或or去掉

当where标签中没有内容，此时where没有任何效果，不会生成任何东西

注意where不能将其中内容后面多余的and或or驱动

```xml
    <select id="getEmpByConditionTwo" resultType="Emp">
        select * from t_emp
        <where>
            <if test="empName !=null and empName != '' ">
                emp_name=#{empName}
            </if>
            <if test="age !=null and age !='' ">
                and age = #{age}
            </if>
            <if test="sex !=null and sex !='' ">
                and sex = #{sex}
            </if>
            <if test="email !=null and email !='' ">
                and email = #{email}
            </if>
        </where>
    </select>
```

### **trim**标签

若标签中有内容时：

prefix：suffix：将trim标签中内容前面或后面添加指定内容

prefixOverrides：suffixOverrides：将trim标签中内容或后面去掉指定内容

若标签中没有内容时：trim标签也没有任何效果

```xml
<select id="getEmpByCondition" resultType="Emp">
    select * from t_emp
    <trim prefix="where"  suffixOverrides="and|or">
        <if test="empName !=null and empName != '' ">
            emp_name=#{empName} and
        </if>
        <if test="age !=null and age !='' ">
            age = #{age} or
        </if>
        <if test="sex !=null and sex !='' ">
            sex = #{sex} and
        </if>
        <if test="email !=null and email !='' ">
            email = #{email}
        </if>
    </trim>
</select>
```

### **choose**、**when**、otherwise标签

相当于if...else  if...else

when--if(){}  if else(){}     otherwise--else(){}

```xml
<select id="getEmpByChoose" resultType="Emp">
        select * from t_emp
        <where>
            <choose>
                <when test="empName !=null and empName != '' ">
                    emp_name = #{empName}
                </when>
                <when test="age !=null and age !='' ">
                    age = #{age}
                </when>
                <when test="sex !=null and sex !='' ">
                    sex = #{sex}
                </when>
                <when test="email !=null and email !='' ">
                    email = #{email}
                </when>
                <otherwise>
                    did=1
                </otherwise>
            </choose>
        </where>
    </select>

```

## **foreach**批量增删

collection：设置需要循环的数组或集合

item：表示数组或集合中的每一个数据

separator：循环体直接的分隔符

open：foreach标签所循环的所有内容的开始符

close：foreach标签所循环的所有内容的结束符

```java
/**
 * 通过数组实现批量删除
 */
int deleteMoreByArray(@Param("eids") Integer[] eids);
```

```xml
    <delete id="deleteMoreByArray">
        delete from t_emp where eid in
        <foreach collection="eids" item="eid" separator="," open="(" close=")">
            #{eid}
        </foreach>
    </delete>
```

```xml
<delete id="deleteMoreByArray">
        delete from t_emp where
        <foreach collection="eids" item="eid" separator="or">
            eid=#{eid}
        </foreach>
</delete>
```

通过集合实现批量增加

```java
/**
 * 通过集合实现批量添加
 */
int insertMoreByList(@Param("emps") List<Emp> emps);
```

```xml
<insert id="insertMoreByList">
    insert into t_emp values
<foreach collection="emps" item="emp" separator=",">
    (null ,#{emp.empName},#{emp.age},#{emp.sex},#{emp.email},null)
</foreach>
</insert>
```

## sql标签

定义sql判断

```xml
<sql id="empColumns">eid,emp_name,age,sex,email</sql>
```

引用sql判断

```xml
<include refid="empColumns"></include>
```

# MyBatis的缓存

缓存只对查询功能有效

将查询出的数据进行保存，再次查询相同数据的时候，就可以直接从缓存中取出

## 一级缓存

默认为一级缓存，SqlSession级别，通过同一个SqlSession，用的不是同一个Mapper对象，但执行的是同一个sql，同样能从一级缓存中获取数据

一级缓存失效的四种情况

不同的SqlSession对应不同的一级缓存，用了不同的SqlSession

同一个SqlSession但是查询条件不同(查询的数据不是同一条)

同一个SqlSession两次查询期间执行了任何一次增删改操作都会清空一级缓存，不情况可能会发生读取到的是改动前的数据。

同一个SqlSession两次查询期间手动清空了缓存`sqlSession.clearCache()`

## 二级缓存

二级缓存需要手动开启，是SqlSessionFactory级别

开启：

在核心配置文件中，设置全局配置属性`catcheEnabled="true"`，默认为true，不需要设置

映射文件中设置标签`<catche/>`

必须在SqlSession关闭或提交之后`sqlSession.commit/close`，数据才会被保存到二级缓存中

查询的数据所转换的实体类类型必须实现序列号接口

二级缓存失效：两次查询直接执行了任意的增删改，(一级和二级缓存同时失效)

### 二级缓存的相关配置

`<catche/>`

eviction

## 缓存查询的顺序

先查询二级缓存，因为二级缓存中可能会有其他程序已经查出来的数据，可以拿来直接使用。

如果二级缓存没有查到，查询一级缓存

一级缓存没有查到，则查询数据库

SqlSession关闭之后，一级缓存中的数据会写入到二级缓存

能用第三方技术代替二级缓存，但一级缓存无法代替	
