dbutil JDBC

需要无参构造

setter—— dbutil JDBC底层进行反射后通过setter给属性赋值

![image-20220316161055464](C:\Users\psyco\AppData\Roaming\Typora\typora-user-images\image-20220316161055464.png)



JavaBean类里的属性名如果和数据库里的表的列名不同，会导致映射反射失败，导致这个属性值未null

映射是根据表的列名调用set***来写入值

给列名一个别名，把属性名也改为别名就能解决
