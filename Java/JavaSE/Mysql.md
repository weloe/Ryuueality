连接到Mysql

mysql -h 主机IP -P 端口 -u 用户名 -p密码

- -p密码不要有空格
- -p后面没有写密码，回车会要求输入密码
- 如果没有写-h，默认就是本机
- 如果没有写-P端口，默认就是3306（实际工作中3306一般会修改

登录前，保证服务启动

net stop mysql服务名

net start mysql服务名

```mysql
C:\windows\system32>cd /D D:\Java\MySQL\mysql-5.7.19-winx64\mysql-5.7.19-winx64\bin

D:\Java\MySQL\mysql-5.7.19-winx64\mysql-5.7.19-winx64\bin>mysql -h localhost -P 3306 -u root -phsp
```

Mysql数据库-普通表的本质仍然是文件



备份数据库（Dos下执行

```mysql
mysqldump -u root -p -B hsp_db02 hsp_db03 > d:\\bak.sql
```

恢复数据库（进入mysql命令行执行

```mysql
source d:\\bak.sql 
```

也可以直接将bac.sql的内容放到查询编辑器中执行



分页查询

limit 每页显示记录数 * (第几页-1) , 每页显示记录数

```mysql
select * from tableName order by id limit 每页显示记录数 * (第几页-1) , 每页显示记录数
```



查询顺序

```mysql
group by -> having -> order by -> limit
```





多表查询默认返回的结果称为笛卡尔集 

```mysql
select * from emp,dept;
```

过滤配对

```mysql
select * from emp,dept
	where emp.deptno = dept.deptno;
```





自连接

把一张表看成两张，给两张表分别取别名` 表明 表别名`，列名不明确，可以指定列的别名`列明 as 列的别名`

用 where 进行过滤

```mysql
SELECT worker.ename AS '职员表', boss.name AS '上级名'
	FROM emp worker, emp boss
	WHERE worker.mgr = boss.empno;
```

