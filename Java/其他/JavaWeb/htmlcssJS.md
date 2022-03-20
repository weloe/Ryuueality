创建web工程-工程下创建html页面

```html
<html><!--表示整个html页面的开始-->
	<head><!--头信息-->
    	<title>标题</title><!--标题-->
	</head>
    <body>
        页面主体内容
    </body>
</html>表示整个html页面的结束
```

标签格式:<标签名>封装的数据</标签名>

```html
<p>开始标签 </p>结束标签  
<br/> 换行 自结束标签
<hr/> 水平线 
<font>字体标签，用来修改文本的字体，颜色，大小(尺寸)
	color属性修改颜色
	face属性修改字体
	size属性修改大小
<  &lt;
>  &gt;
空格 &nbsp;
<h1>标题1</h1> h1-h6都是标题标签，h1最大 h6最小
    align对其属性 left center right 默认左对齐


```





a超链接标签

```html
<a>超链接</a>
    href属性 设置链接地址
	target属性设置哪个目标进行跳转 
		_self  当前页面(默认)
		_blank 打开新页面进行跳转 
<a href="http://www.baidu.com">baidu</a> 当前页面跳转

```





列表标签：有序列表ol(orderlist)、无序列表ul

ul 属性type可改列表项前面的符号

li是列表项

```html
<ul>
    <li>赵1</li>
    <li>赵2</li>
    <li>赵3</li>
    <li>赵4</li>
</ul>

<ol>
    <li>赵1</li>
    <li>赵2</li>
    <li>赵3</li>
    <li>赵4</li>
</ol>
```

imag标签

可以在html页面上显示图片

src属性设置图片的路径

JavaSE中路径也分为相对路径和绝对路径

相对路径：从工程名开始算

绝对路径：盘符:/目录/文件名

web中也分为两种

 相对路径：

.	表示当前文件所在的目录

.. 	表示当前文件所在的上一级目录

文件名 	当前文件所在目录的文件，相当于 ./文件名      ./   可以省略

绝对路径格式：  http://ip:port/工程名/资源路径

```html
<img src="../imgs/1.jpg" width="100" height="122" border="2" alt="找不到"/>
```

wideth height 设置宽、高，border指定边框的像素，alt 当图片找不到时用来显示的文本内容





表格标签

```html
<table border="1">
    <tr>
        <td>1.1</td>
        <td>1.2</td>
        <td>1.3</td>
    </tr>
    <tr>
        <td>2.1</td>
        <td>2.2</td>
        <td>2.3</td>
    </tr>
    <tr>
        <td>3.1</td>
        <td>3.2</td>
        <td>3.3</td>
    </tr>
</table>
```





```html
<td align="center"><b>1.1</b></td>
<th>1.2</th>
```

table标签是表格标签

​	border 设置表格边框 width 设置表格宽度	height 设置表格高度 align设置表格相对于页面对齐

​	cellspacing 设置单元格间距

tr 是行标签

th 是表头标签

td 是单元格标签

​	**colspan设置跨几列**

​	**rowspan设置跨几行**

​	align设置单元格文本对齐

b 是加粗标签





iframe框架标签(内嵌窗口)--可以在一个html界面上，打开一个小窗口，去加载一个单独的页面

要双标签

ifram和a标签组合使用步骤

1.在ifram标签中使用name属性定义一个名称

2.在a标签的target属性上设置ifram的name的属性值

```html
我是一个单独完整的页面<br/><br/>
<iframe src="hello.html" width="500" height="400" name="abc"></iframe>

<ul><li><a href="hello.html" target="abc">hello</a></li>
```





表单标签

html页面中用来收集用户信息的所有元素集合，然后把这些信息发送给服务器

form标签就是表单

input type=text 是文本输入框 value设置默认显示内容

input type=password 是密码输入框 value设置默认显示内容

input type=radio 是单选框   name属性可以对其进行分组  check="check"表示默认选中(单选框不能加多个) 

input type=checkbox 是复选框  check="check"(可以加多个)



select 标签是下拉列表框

option 标签是下拉列表框中的选项 	selected="selected"设置默认选中，不设置默认选择第一个option

textera表示多行文本输入框 	rows属性设置可以显示几行的高度 	cols属性设置每行可以显示几个字符宽度	 起始标签和结束标签中的内容是默认值

input type=resert 是重置按钮  value属性修改按钮上的文本  重置为设置的默认值或者默认的

input type=submit 是提交按钮  value属性修改按钮上的文本

input type=button 是按钮  value属性修改按钮上的文本

input type=file 是文件上传域

input type=hidden 是隐藏域  当我们要发送某些信息，而这些信息，不需要用户参与，就可以使用隐藏域(提交的时候同时发送给服务器)

```html
	用户名称：<input type="text" value="默认值"/><br/>
        密码：<input type="password" value="abc" /><br/>
        确认密码：<input type="password" value="abc" /><br/>
        性别：
        <input type="radio" name="sex" checked="checked"/>男
        <input type="radio" name="sex"/>女 <br>
        兴趣爱好：
        <input type="checkbox" checked="checked"/>Java
        <input type="checkbox" checked="checked"/>JavaScrip
        <input type="checkbox" checked="checked"/>C++ <br/>
        国籍：<select>
                <option>---请选择国籍---</option>
                <option selected="selected">---1---</option>
                <option>---2---</option>
                <option>---3---</option>
                </select><br/>
        自我评价：<textarea rows="10" cols="20">默认值</textarea><br/>
        <input type="reset"/>
        <input type="submit"/>
        <input type="button" value="按钮"/>
        <input type="file"/>
        <input type="hidden"/>
```

用table、tr、td标签格式化表单

```html
         <table align="center">
            <tr>
                <td>用户名称：</td>
                <td><input type="text" value="默认值"/></td>
            </tr>
            <tr>
                <td>密码：</td>
                <td><input type="password" value="abc" /></td>
            </tr>
            <tr>
                <td>确认密码：</td>
                <td><input type="password" value="abc" /></td>
            </tr>
            <tr>
                <td>性别：</td>
                <td>
                    <input type="radio" name="sex" checked="checked"/>男
                    <input type="radio" name="sex"/>女
                </td>
            </tr>
            <tr>
                <td>兴趣爱好：</td>
                <td>
                    <input type="checkbox" checked="checked"/>Java
                    <input type="checkbox" checked="checked"/>JavaScrip
                    <input type="checkbox" checked="checked"/>C++
                </td>
            </tr>
            <tr>
                <td>国籍：</td>
                <td>
                    <select>
                    <option>---请选择国籍---</option>
                    <option selected="selected">---1---</option>
                    <option>---2---</option>
                    <option>---3---</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>自我评价：</td>
                <td>
                    <textarea rows="10" cols="20">默认值</textarea>
                </td>
            </tr>
            <tr>
                <td><input type="reset"/></td>
                <td align="center"><input type="submit"/></td>
            </tr>
        </table>
```





表单发送细节

form标签是表单标签

action属性设置提交到服务器地址

method属性设置提交的方式GET(默认值)或POST

表单提交的时候，数据没有发送给服务器的三种情况：

1. 表单项没有name属性值
2. 单选、复选(下拉列表中的option标签 )  都需要添加value属性以便发给服务器
3. 表单项不在提交的form标签中

GET请求的特点：

1. 浏览器地址栏中的地址是：action属性[+？+请求参数] 请求参数的格式是：name=value&name=value
2. 不安全
3. 有数据长度的限制

POST请求的特点

1. 浏览器地址栏中只有action属性值
2. 比GET请求安全
3. 理论上没有数据长度的限制





div、span、p标准的演示

div标签默认独占一行

span标签	它的长度是封装数据的长度

p段落标签   默认在段落的上方或下方各空出一行（如果已有就不再空）





CSS三种写法

第一种：在标签加style设置

第二种 写在head标签之间

```html
<style type="text/css">
        div{
            border: 1px solid red;
        }
    </style>
```

第三种：把css样式写成单独的css文件，再通过html标签引入即可复用

link标签专门用来引入css样式代码

```html
    <link rel="stylesheet" type="text/css" href="1.css"/>
```





标签名选择器的格式：

```
标签名{
	属性:值;
}
```

可以决定哪些标签被动的使用这个样式





id选择器的格式

```
#id属性值{
	属性:值;
}
```

可以让我们通过id属性选择性的去使用这个样式

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Title</title>
    <style type="text/css">
        #id001{
            color: blue;
            font-size: 20px;
            border: 20px solid yellow;
        }
        #id002{
            color: red;
            font-size: 20px;
            border: 5px blue dotted;
        }
    </style>
</head>
<body>
	<div id="id001">div标签1</div>
	<div id="id002">div标签2</div>

</body>
</html>
```





class选择器

```
.class 属性值{
	属性:值;
}
```

可以通过class属性有效的选择性的去使用这个样式

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Title</title>
    <style type="text/css">
        #id001{
            color: blue;
            font-size: 20px;
            border: 20px solid yellow;
        }
        #id002{
            color: red;
            font-size: 20px;
            border: 5px blue dotted;
        }
        .class01{
            color: blue;
            font-size: 30px;
            border: 1px solid yellow;
        }
        .class02{
            color: grey;
            font-size: 26px;
            border: 1px solid red;
        }


    </style>
</head>
<body>
	<div id="id001">div标签1</div>
	<div id="id002">div标签2</div>
	<span class="class01">span标签1</span>
	<span class="class02">span标签2</span>
</body>
</html>
```





组合选择器格式

```
选择器1,选择器2,选择器n{
	属性:值;
}
```

可以让多个选择器共用同一个css样式代码





JavaScript

JavaScript语言诞生主要是完成页面的数据验证。因此它运行在客户端，需要运行浏览器来解析执行

JavaScript代码。JS是弱类型(类型可变)，Java是强类型(定义变量时类型已确定，不可变)。

交互性(做信息交互)、安全性(不允许直接访问本地硬盘)、跨平台性



JavaScript于HTML结合使用

第一种方式 

在head或者body标签中

```html
<script type="text/javascript">
        alert("he js");
    </script>
```

第二种

使用script标签引入单独的JavaScript代码文件 

script标签可以用来定义js代码，也可以用来引入js文件但是，两个功能二选一使用。不能同时使用两个功能

```html
<scripttype="text/javascript"src="1.js"></script>

<scripttype="text/javascript">
alert("555");
    </script
```





变量

JavaScript变量类型：

数值类型： number

字符串类型：String

对象类型：Object

布尔类型：boolean

函数类型：function

特殊的值：

undefined  未定义，所有js变量未赋于初始值时，默认值都是undefined

null	空值

NAN	Not a Number 非数字，非数值

var 变量名;		var 变量名 = 初始值;





关系(比较)运算

等于： == 等于是简单的字面值的比较

全等于： === 除了做字幕组的比较之外还好比较两个变量的数据类型





逻辑运算

且运算：&&

或运算：||

取反运算：!

在JavaScript中所有的变量都可以作为一个boolean类型的变量去使用

0、null、undefined、""(空串) 都认为是false

&&且运算。有两种情况：

第一种：当表达式全为真的时候。返回最后一个表达式的值。

第二种：当表达式中，有一个为假的时候。返回第一个为假的表达式的值

||或运算

第一种情况：当表达式全为假时，返回最后一个表达式的值

第二种情况：只要有一个表达式为真。就会把回第一个为真的表达式的值

并且&&与运算和||或运算有短路。短路就是说，当这个&&或||运算有结果了之后。后面的表达式不再执行





数组

只要我们通过数组下标赋值(读值不行)，那么最大的下标值就会自动给数组做扩容操作





函数

第一种定义方式：function 函数名(形参列表){ 函数体 }

有参函数不需要加参数类型

定义带有返回值的函数： 在函数体内直接使用return语句返回值即可

第二种：var 函数名 = function(形参列表){ 函数体 }

JS中函数的重载会直接覆盖调上一次的定义

无参函数也能传入参数

隐形参数(只在function函数内)

就是在function函数中不需要定义，但却可以直接用来获取所有参数的变量，隐形参数类似Java基础的可变长参数

js中的隐形参数也跟java的可变长参数一样，操作类似数组。

```html
function fun(){
    alert(arguments.length);//可看参数个数
    //alert(arguments[0]);
    //alert(arguments[1]);
    //alert(arguments[2]);
    for (var i = 0; i<arguments.length;i++){
        alert(arguments[i]);
    }
    alert("无参函数fun()");
}
//fun(1,"ad",true);

function sum(num1,num2) {
    var result = 0;
    for(var i = 0 ; i < arguments.length ; i++){
        
        result +=arguments[i];

    }
    return result;
}
alert(sum(1,2,3,4,5,6,7,8));
alert(sum(1,2,3,"abc",5,6,7,8));
```





JS中的自定义对象

Object形式的自定义对象

对象的定义：

var 变量名= new Object();   //对象实例(空对象)

变量名.属性 = 值;    //定义一个属性

变量名.函数名 = function(){  }  //定义一个函数

对象的访问： 

变量名.属性/函数名();

{}花括号形式的自定义对象

var 变量名 = { };

var 变量名 = {  属性名:值,属性名:值,函数名:function(){ }  };

对象的访问： 

变量名.属性/函数名();





js中的事件

onload 加载完成实际，页面加载完成之后，常用于做页面js代码初始化操作

onclick 单击事件，常用于按钮的点击响应操作

onblur 失去焦点事件，常用于输入框失去焦点后验证其输入内容是否合法

onchange 内容发生改变事件，常用于下拉列表和输入框内容发生改变后操作

onsubmit 表单提交事件，常用于表单提交前，验证所有表单项是否合法





事件的注册(绑定)

告诉浏览器，当事件响应后要执行哪些操作代码，叫事件注册或绑定。

静态注册事件：通过html标签的事件属性直接赋于事件响应后的代码，这种方式我们叫静态注册

动态注册事件：先通过js代码得到标签的dom对象，然后再通过dom对象.事件名 = function(){}  这种形式赋于事件响应后的代码，叫动态注册

动态注册基本步骤：

1.获取标签对象

2.标签对象。事件名 = function(){}

静态注册onload事件是浏览器解析完页面之后就好自动触发的事件（登陆页面后触发）











