# JUC并发基础

## 基础概念

### 线程、进程

### 线程的状态

### wait和slepp区别

### 并发，并行

### 管程

monitor监视器(锁)

一种同步机制，保证同一时间只能有一个线程访问被保护的数据或代码

jvm同步基于进入和退出，使用管程对象实现

### 用户线程和守护线程

用户线程：自定义线程

守护线程：比如垃圾回收

主线程结束，用户线程还在运行，jvm存活

如果没有用户，都是守护线程，jvm结束

## synchronized和lock

### 同步锁

修饰代码块，被修饰的代码块称为同步语句块，作用的范围是大括号括起来的代码，作用的对象是调用这个代码块的对象

修饰一个方法，被修饰的方法被称为同步方法，作用对象是调用这个方法的对象

修饰一个静态方法，作用的范围是整个静态方法，作用的对象是这个类的所有对象

### lock与synchronized区别

- Lock不是Java语言内置的，synchronized是Java语言的关键字，因此是内置特性。Lock是一个类，通过这个类可以实现同步访问.
- synchronized不需要用户取手动是否锁，当synchronized方法或者synchronized代码块执行完后，系统会自动让线程释放对锁的占用;而Lock则必须要用户手动释放锁，如果没有主动释放锁，就有可能导致出现死锁现象
- Lock可以让等待锁的线程响应中断，而synchronized却不行，使用synchronized时，等待的线程会一直等待下去，不能够响应中断
- 通过Lock可以知道有没有成功获取锁，而synchronized却无法办到
- Lock可以提高多个线程进行读操作的效率

```java
//创建资源类，定义属性和方法
class Ticket {
    private int number = 30;
    public synchronized void sale() {
        //
        if(number > 0) {
            System.out.println(Thread.currentThread().getName()+"：卖出："+(number--)+"剩下："+number);
        }
    }
}

public class SaleTicket {
    //创建多个线程
    public static void main(String[] args) {
        //创建ticket对象
        Ticket ticket = new Ticket();
        //创建线程
        new Thread(new Runnable() {
            @Override
            public void run() {
                for (int i = 0; i <40 ; i++) {
                    ticket.sale();
                }
            }
        },"AA").start();
        new Thread(new Runnable() {
            @Override
            public void run() {
                for (int i = 0; i <40 ; i++) {
                    ticket.sale();
                }
            }
        },"BB").start();
        new Thread(new Runnable() {
            @Override
            public void run() {
                for (int i = 0; i <40 ; i++) {
                    ticket.sale();
                }
            }
        },"CC").start();
    }
}
```

```java
//创建资源类，定义属性和方法
class LTicket {
    private int number = 30;

    //创建可重入锁
    private final ReentrantLock lock = new ReentrantLock();

    public void sale() {
        //上锁
        lock.lock();

        try {
            //判断是否有票
            if(number > 0) {
                System.out.println(Thread.currentThread().getName()+"：卖出："+(number--)+"剩下："+number);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }finally {
            //解锁
            lock.unlock();
        }

    }
}
public class LSaleTicket {


    public static void main(String[] args) {
        LTicket ticket = new LTicket();
        //创建三个线程
        new Thread(()->{
            for (int i = 0; i < 40; i++) {
                ticket.sale();
            }
        },"AA").start();
        new Thread(()->{
            for (int i = 0; i < 40; i++) {
                ticket.sale();
            }
        },"BB").start();
        new Thread(()->{
            for (int i = 0; i < 40; i++) {
                ticket.sale();
            }
        },"CC").start();

    }

}
```

### 线程间通信synchronized

```java
class Share {
    private int number = 0;

    //+1的方法
    public synchronized void incr() throws InterruptedException {
        //判断 do 通知
        if (number != 0) { //不是0不+1
            this.wait();
        }
        //如果是0
        number++;
        System.out.println(Thread.currentThread().getName() + "::" + number);

        //通知其他线程
        this.notifyAll();

    }

    //-1的方法
    public synchronized void decr() throws InterruptedException {
        //判断
        if (number != 1) {
            this.wait();
        }
        //do
        number--;
        System.out.println(Thread.currentThread().getName() + "::" + number);

        //通知其他线程
        this.notifyAll();
    }

}

public class ThreadDemo1 {

    public static void main(String[] args) {
        Share share = new Share();
        new Thread(() -> {
            for (int i = 0; i < 10; i++) {
                try {
                    share.incr();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }, "AA").start();
        new Thread(() -> {
            for (int i = 0; i < 10; i++) {
                try {
                    share.decr();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }, "BB").start();

    }
}
```

虚假唤醒问题：wait时释放锁，别的线程进入抢到资源，if只判断一次

AA BB CC DD

AA+1 BB-1 CC+1 DD-1

![image-20220617153042647](JUC%E5%B9%B6%E5%8F%91.assets/image-20220617153042647.png)

**解决方法：条件判断需要写在循环中**

### Lock实现线程间通信

```java
class Share {
    private int number = 0;

    private Lock lock = new ReentrantLock();
    private Condition condition = lock.newCondition();

    //+1
    public void incr() throws InterruptedException {
        //上锁
        lock.lock();

        try {
            //判断
            while (number != 0 ) {
                condition.await();//等待
            }
            //do
            number ++;
            System.out.println(Thread.currentThread().getName() + "::" + number);

            //通知
            condition.signalAll();
        }finally {
            //解锁
            lock.unlock();
        }

    }

    //-1
    public void decr() throws InterruptedException {
        lock.lock();
        try {
            while (number != 1) {
                condition.await();
            }
            number -- ;
            System.out.println(Thread.currentThread().getName() + "::" + number);

            //通知
            condition.signalAll();

        }finally {
            lock.unlock();
        }

    }


}


public class ThreadDemo2 {

    public static void main(String[] args) {
        Share share = new Share();
        new Thread(() -> {
            for (int i = 0; i < 10; i++) {
                try {
                    share.incr();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }, "AA").start();
        new Thread(() -> {
            for (int i = 0; i < 10; i++) {
                try {
                    share.decr();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }, "BB").start();
        new Thread(() -> {
            for (int i = 0; i < 10; i++) {
                try {
                    share.incr();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }, "CC").start();
        new Thread(() -> {
            for (int i = 0; i < 10; i++) {
                try {
                    share.decr();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }, "DD").start();
    }
}
```

### 线程间的定制化通信

启动三个线程，让三个线程按照需求执行

AA打印5次，BB打印10次，CC打印15次

进行10轮

![image-20220617171423372](JUC%E5%B9%B6%E5%8F%91.assets/image-20220617171423372.png)

```java
class ShareResource {
    //定义标志位
    private int flag = 1;//1 AA 2BB 3CC
    //创建Lock
    private Lock lock = new ReentrantLock();

    //创建三个condition
    private Condition c1 = lock.newCondition();
    private Condition c2 = lock.newCondition();
    private Condition c3 = lock.newCondition();


    //打印5次，参数第几轮
    public void print5(int loop) throws InterruptedException {
        //上锁
        lock.lock();
        try {
            while (flag != 1) {
                c1.await();
            }
            for (int i = 1; i <= 5; i++) {
                System.out.println(Thread.currentThread().getName() + "::" + i + ":轮数:" +loop);
            }
            flag=2;//修改标注位
            c2.signal();//通知BB线程
        }finally {
            lock.unlock();
        }

    }

    //打印10次，参数第几轮
    public void print10(int loop) throws InterruptedException {
        //上锁
        lock.lock();
        try {
            while (flag != 2) {
                c2.await();
            }
            for (int i = 1; i <= 10; i++) {
                System.out.println(Thread.currentThread().getName() + "::" + i + ":轮数:" +loop);
            }
            flag=3;//修改标注位
            c3.signal();//通知BB线程
        }finally {
            lock.unlock();
        }
    }

    //打印15次，参数第几轮
    public void print15(int loop) throws InterruptedException {
        //上锁
        lock.lock();
        try {
            while (flag != 3) {
                c3.await();
            }
            for (int i = 1; i <= 15; i++) {
                System.out.println(Thread.currentThread().getName() + "::" + i + ":轮数:" +loop);
            }
            flag=1;//修改标注位
            c1.signal();//通知BB线程
        }finally {
            lock.unlock();
        }
    }


}



public class ThreadDemo3 {

    public static void main(String[] args) {
        ShareResource shareResource = new ShareResource();
        new Thread(() -> {
            for (int i = 1; i <= 10; i++) {
                try {
                    shareResource.print5(i);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }, "AA").start();
        new Thread(() -> {
            for (int i = 1; i <= 10; i++) {
                try {
                    shareResource.print10(i);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }, "BB").start();
        new Thread(() -> {
            for (int i = 1; i <= 10; i++) {
                try {
                    shareResource.print15(i);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }, "CC").start();

    }
}
```

## 集合线程安全

### List

#### ArrayList

线程不安全的集合

```java
public class ThreadDemo4 {

    public static void main(String[] args) {
        List<String> list = new ArrayList<>();
        for (int i = 0; i < 10; i++) {
            new Thread(()->{
                //向集合添加内容
                list.add(UUID.randomUUID().toString().substring(0,8));
                //从集合获取内容
                System.out.println(list);
            },String.valueOf(i)).start();
        }
    }
}
```

![image-20220617174016851](JUC%E5%B9%B6%E5%8F%91.assets/image-20220617174016851.png)

#### Vector

线程安全

#### Collections

```java
List<> list = Collections.synchronizedList(new ArrayList<>());
```

#### CopyOnWriteArray

线程安全

```java
CopyOnWriteArrayList<Object> list = new CopyOnWriteArrayList<>();
```

写时复制技术

读的时候直接读，修改时先复制原集合，更改复制出来的集合，再把更改后的集合复制回原集合

### Set

#### HashSet

HashSet底层是HashMap

线程不安全

#### CopyOnWriteArraySet

线程安全

### Map

#### HashMap

### ConcurrentHashMap

## 锁

### 锁的范围

方法上加锁，锁的范围是这个对象(this)

静态方法加锁，锁的范围是Class，字节码对象

方法块加锁，锁的范围是Synchronized括号里的对象

### 公平锁和非公平锁

公平锁：不会造成线程饿死，效率较低

非公平锁：会造成线程饿死，执行效率高

### 可重入锁(递归锁)

synchronized(隐式)和lock(显式)都是可重入锁

### 死锁

两个或者两个以上进程在执行过程中，因为争夺资源造成的互相的等待的现象，如果没有外力干涉，他们就无法再执行下去。

死锁原因：系统资源不足，进程运行推进顺序不合适，资源分配不当

```java
public class DeadLock {
    static Object a = new Object();
    static Object b = new Object();

    public static void main(String[] args) {
        new Thread(()->{
            synchronized (a) {
                System.out.println(Thread.currentThread().getName()+"持有锁a，试图获取锁b");
                synchronized (b) {
                    System.out.println(Thread.currentThread().getName()+"获取锁b");
                }
            }
        },"A").start();

        new Thread(()->{
            synchronized (b) {
                System.out.println(Thread.currentThread().getName()+"持有锁b，试图获取锁a");
                synchronized (a) {
                    System.out.println(Thread.currentThread().getName()+"获取锁a");
                }
            }
        },"B").start();

    }
}
```

![image-20220618144655707](JUC%E5%B9%B6%E5%8F%91.assets/image-20220618144655707.png)

怎么验证是否是死锁？

1. `jps`          类似 linux的 ps-ef
2. `jstack 进程`     jvm自带的堆栈跟踪工具

![image-20220618145214583](JUC%E5%B9%B6%E5%8F%91.assets/image-20220618145214583.png)

## Callable接口

比继承Thread和实现Runnable接口创建线程多的功能是可以使线程返回结果。

Runnable接口和Callable接口



1. 是否有返回值
2. 是否抛出异常（call方法，无法计算结果会抛出异常）
3. 实现方法名称不同，一个是run方法，一个是call方法

## 辅助类

### 减少技术CountDownLatch 

```java
public class CountDownLatchDemo {
    public static void main(String[] args) throws InterruptedException {

        CountDownLatch countDownLatch = new CountDownLatch(6);

        //6个同学陆续离开教师
        for (int i = 1; i <= 6; i++) {
            new Thread(() -> {
                System.out.println(Thread.currentThread().getName() + " 号同学离开了教室");
                countDownLatch.countDown();//计数器值减一

            }, String.valueOf(i)).start();
        }

        //等待
        countDownLatch.await();
        System.out.println(Thread.currentThread().getName() + " 班长锁门");

    }
}
```

### 循环栅栏CyclicBarrier

```java
public class CyclicBarrierDemo {

    //创建固定值
    private static final int NUMBER = 7;
    public static void main(String[] args) {
        //创建CycliBarrier
        CyclicBarrier cyclicBarrier = new CyclicBarrier(NUMBER, () -> {
            System.out.println("7");
        });

        //集齐7
        for (int i = 1; i <= 7 ; i++) {
            new Thread(()->{
                System.out.println(Thread.currentThread().getName() + "收集");

                //等待
                try {
                    cyclicBarrier.await();
                    System.out.println(1);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                } catch (BrokenBarrierException e) {
                    e.printStackTrace();
                }
            },String.valueOf(i)).start();
        }

    }
}
```

### 信号灯Semaphore

```java
//6辆汽车，停3个车位
public class SemaphoreDemo {
    public static void main(String[] args) {
        //创建Semaphore，设置许可数量
        Semaphore semaphore = new Semaphore(3);
        
        //模拟6辆汽车
        for (int i = 1; i <= 6; i++) {
            new Thread(()->{
                //占车位
                try {
                    semaphore.acquire();

                    System.out.println(Thread.currentThread().getName()+" 抢到车位");
                    //设置随机停车时间
                    TimeUnit.SECONDS.sleep(new Random().nextInt(5));

                    System.out.println(Thread.currentThread().getName()+"---离开了车位");
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }finally {
                    //释放
                    semaphore.release();

                }
            },String.valueOf(i)).start();
        }
    }
}
```

## ReentrantReadWriteLock读写锁

一个资源可以被多个读线程访问，或者可以被一个写线程访问，但是不能同时存在读写线程，读写互斥，读读共享

第一无锁情况：多线程抢夺资源

第二添加锁：synchronized和ReentrantLock ,都是独占的，每次只能一个操作

第三读写锁：ReentrantReadWriteLock，读读操作共享，提升性能，写只能一个操作

缺点

- 造成锁饥饿，一直读，没有写操作
- 读时，不能进行写操作，只有读完才能写，写操作完才能读

读写锁的降级：将写入锁降级为读锁





### 悲观锁



### 乐观锁

改动数据后改变版本号，每次改动先对比版本号是否一致

### 表锁

操作一个表时给表上锁，其他人就不能操作

只能一个人()操作表

### 行锁

只能一个人操作行数据，会发生死锁

### 读锁

共享锁，会发生死锁

### 写锁

独占锁，会发生死锁 

```java
class MyCache {

    //创建map集合
    private volatile Map<String,Object> map = new HashMap<>();

    //创建读写锁对象
    private ReadWriteLock rwLock = new ReentrantReadWriteLock();

    //放数据
    public void put(String key,Object value) {
        //添加写锁
        rwLock.writeLock().lock();

        System.out.println(Thread.currentThread().getName() + " 正在写操作"+key);

        //
        //放数据
        map.put(key,value);

        System.out.println(Thread.currentThread().getName() + " 写完了"+key);

        try {
            TimeUnit.MICROSECONDS.sleep(300);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }finally {
            rwLock.writeLock().unlock();
        }

}

    //取数据
    public Object get(String key) {
        //添加读锁
        rwLock.readLock().lock();

        Object result = null;
        try {

            System.out.println(Thread.currentThread().getName() + " 正在读取"+key);

            TimeUnit.MICROSECONDS.sleep(300);
            result = map.get(key);
            System.out.println(Thread.currentThread().getName() + " 取完了"+key);

        } catch (InterruptedException e) {
            e.printStackTrace();
        }finally {
            rwLock.readLock().unlock();
        }

        return result;
    }
}
public class ReadWriteLockDemo {
    public static void main(String[] args) {
        MyCache myCache = new MyCache();
        //创建线程是否数据
        for (int i = 1; i <= 5; i++) {
            final int num = i;
            new Thread(()->{
                myCache.put(num+"",num+"");
            },String.valueOf(i)).start();
        }

        for (int i = 1; i <= 5; i++) {
            final int num = i;
            new Thread(()->{
                myCache.get(num+"");
            },String.valueOf(i)).start();
        }
    }
}
```

## BlockingQueue阻塞队列

### ArrayBlockingQueue

基于数组的阻塞队列实现，在ArrayBlockingQueue内部，维护了一个定长数组

### LinkedBlockingQueue

链表组成的有界阻塞队列

### DelayQueue

使用优先级队列实现的延迟无界阻塞队列

### PriorityBlockingQueue



### SynchronousQueue

不存储元素的阻塞队列，也即单个元素的队列

## 线程池

控制运行的线程数量，处理过程将任务放入队列，然后在线程创建后启动这些任务，如果线程数量超过了最大数量，超出数量的线程排队等候，等其他线程执行完毕，再从队列中取出任务来执行。

## 分支合并框架(Fork/Join)

## CompletableFuture异步回调

# JUC进阶

## 基础概念

### Start线程启动

start同步方法->start0()

`private native void start0();`第三方c语言编写的底层函数或者是操作系统的底层代码

Java语言本身底层就是C++语言

调了thread.c，jvm.cpp，thread.cpp

### 多线程概念

并发，并行

进程，线程，管程

### 用户线程和守护线程

不做特别说明配置都是用户线程

用户线程：系统的工作线程，它会完成这个程序需要完成的业务操作

守护线程：为其他线程服务的线程。如果用户线程全部结束，守护线程也会结束	 例如 GC垃圾回收线程

## CompletableFuture

### Future

Future接口(FutureTask实现类)定义了操作异步任务执行一些方法，如获取异步任务的执行结果、取消任务的执行、判断任务是否被取消、判断任务是否完毕等。

Future可以为主线程开一个分支任务，专门为主线程处理耗时和费力的复杂业务

### FutureTask

