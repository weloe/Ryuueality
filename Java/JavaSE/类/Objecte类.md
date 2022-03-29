属性没有重写之说，属性的值看编译类型

instanceof 比较操作符，用于判断对象的运行类型是否为XX类型或XX类型的子类型	



# Objecte类

clone()

equals(Object obj)

finalize()

getClass()

hashCode()

notify()

notifyAll()

toString()

wait()

wait(long timeout)

wait(long timeout,int nanos)

## ==和equals

==

- 可以判断基本类型和引用类型
- 如果判断基本类型，判断的是值是否相等。
- 如果判断引用类型，判断的是地址是否相等，即判断是不是同一个对象

```Java
public class Equals {
    public static void main(String[] args) {

        A a = new A();
        A b = a;
        A c = b;
        System.out.println(a==c);
        B bObj = a;
        System.out.println(bObj==c);
    }
}
class B{

}
class A extends B{

}
```





equals

只能判断引用类型

Object的equals方法默认是比较对象的地址是否相同，即判断两个对象是否是同一个

```java
public boolean equals(Object obj) {
        return (this == obj);
    }
```

String的equals方法把Object的equals方法重写了，变成了比较两个字符串的值是否相等

Integer也重写了equals，比较的是值是否相等

## hashCode



## toString

默认返回：全类名+@+哈希值的十六进制

全类名——包名加类名

直接输出一个对象时，toString方法会被默认的调用

`System.out.println(objectName)`默认调用objectName.toString()



## finalize

