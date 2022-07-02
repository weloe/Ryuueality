# SpringCloud

## Eureka

RestTemplate远程调用



Ribbon

负载均衡

`http://userservice/user/1`根据服务名称后从`eureka-server`中拉取服务ip端口`localhost:8081`开始轮询

@LoadBalanced标记RestTemplate发起的请求要被Ribbon(类LoadBalancerInterceptor)拦截处理

LoadBalancerInterceptor 实现 ClientHttpRequestInterceptor接口 拦截由客户端发起的请求

拦截后根据request的uri和host(服务昵称userservice)，把服务昵称给RibbonLoadBalanceClient执行，再通过`getLoadBalancer(serviceId)`得到动态服务列表均衡器(得到服务列表)

->`getServer(loadBalancer,hint)`->chooseServer->super.chooseServer根据IRuler接口的实现类(默认为ZoneAvoidanceRule)规定的规则(轮询？随机？)获得端口号，再用真实的ip和port来代替userservice(服务昵称)发起请求

![image-20220620193711578](SpringCloud.assets/image-20220620193711578.png)

![image-20220620195016212](SpringCloud.assets/image-20220620195016212.png)

![image-20220620195035934](SpringCloud.assets/image-20220620195035934.png)

![image-20220628223917501](SpringCloud.assets/image-20220628223917501.png)

## Nacos

为什么要集群：防止出现跨集群调用

服务调用尽可能选择本地集群的服务，跨集群调用延迟较高

```yml
userservice:
  ribbon:
    NFLoadBalancerRuleClassName: com.alibaba.cloud.nacos.ribbon.NacosRule # 优先随机访问本集群，如果本地集群没有会访问其他集群
```

可设置IP访问权重，权重大的被访问频率高，权重为0表示不访问这个IP

### 环境隔离

Namespace-》 Group-》 Service/Data

Nacos中服务存储和数据存储的最外层都是一个名为namespace的东西，用来做最外层隔离

用于环境变化隔离，开发环境，生产环境

- namespace用于环境隔离
- 每个namespace都有唯一id
- 不同namespace下的服务不可见

临时实例采用心跳检测，每隔一段时间发送到Nacos验证服务是否正常，不正常剔除(和Eureka一样)

非临时实例 nacos主动询问，如果不正常会标红，不剔除

nacos注册中心比eureka多了消息推送，如果服务有变动(例如挂了一个服务)会立即推送给服务消费者

![image-20220629144433397](SpringCloud.assets/image-20220629144433397.png)

![image-20220629144734260](SpringCloud.assets/image-20220629144734260.png)

### 统一配置管理

#### Nacos配置管理

#### 微服务配置拉取

#### 配置热更新

#### 多环境配置共享

不同环境的相同配置属性放到userservice.yaml中

优先级：

服务名-profile.yaml > 服务名称.yaml > 本地配置

nacos中的配置大于本地配置

### Nacos集群搭建

![image-20220701130456722](SpringCloud.assets/image-20220701130456722.png)

## Feign

RestTemplate方式调用存在问题

代码可读性差，编程体验不统一

参数复杂的url难以维护

### 远程调用

Feign 声明式的http客户端 ，帮助我们优雅的实现http请求的发送，解决上面提到的问题

- 引入依赖
- 添加@EnableFeignClient注解
- 编写FeignClient接口
- 使用FeignClient中定义的方法代替RestTemplate

### 自定义Feign配置

![image-20220701133002694](SpringCloud.assets/image-20220701133002694.png)

![image-20220701140023609](SpringCloud.assets/image-20220701140023609.png)

### Feign的性能优化

![image-20220701140349114](SpringCloud.assets/image-20220701140349114.png)

![image-20220701140419411](SpringCloud.assets/image-20220701140419411.png)

![image-20220701140757530](SpringCloud.assets/image-20220701140757530.png)

### Feign的最佳实践

![image-20220701141124006](SpringCloud.assets/image-20220701141124006.png)

但是一般不推荐在客户端和服务端之间共享接口，会造成紧耦合(API层面耦合),对mvc不起作用，方法参数无法继承下来

![image-20220701141710901](SpringCloud.assets/image-20220701141710901.png)

但是会造成引入多余的方法client方法

![image-20220701141813199](SpringCloud.assets/image-20220701141813199.png)

![image-20220701142034936](SpringCloud.assets/image-20220701142034936.png)

![image-20220701142854084](SpringCloud.assets/image-20220701142854084.png)

## Gateway

### 基本使用

![image-20220701143734552](SpringCloud.assets/image-20220701143734552.png)

![image-20220701143827871](SpringCloud.assets/image-20220701143827871.png)

![image-20220701143846846](SpringCloud.assets/image-20220701143846846.png)

![image-20220701144738403](SpringCloud.assets/image-20220701144738403.png)

![image-20220701145429247](SpringCloud.assets/image-20220701145429247.png)

![image-20220701145517377](SpringCloud.assets/image-20220701145517377.png)

### 路由断言

![image-20220701150527034](SpringCloud.assets/image-20220701150527034.png)

![image-20220701150538825](SpringCloud.assets/image-20220701150538825.png)

### 路由过滤器

![image-20220701151323094](SpringCloud.assets/image-20220701151323094.png)

![image-20220701151711539](SpringCloud.assets/image-20220701151711539.png)

![image-20220701152144623](SpringCloud.assets/image-20220701152144623.png)

![image-20220701152155701](SpringCloud.assets/image-20220701152155701.png)

![image-20220701152419597](SpringCloud.assets/image-20220701152419597.png)

#### 全局过滤器GlobalFilter

和defaultFilters作用一样

![image-20220701165821054](SpringCloud.assets/image-20220701165821054.png)

### 过滤器执行顺序

![image-20220701170005814](SpringCloud.assets/image-20220701170005814.png)

![image-20220701170958998](SpringCloud.assets/image-20220701170958998.png)![image-20220701171029448](SpringCloud.assets/image-20220701171029448.png)![image-20220701171329457](SpringCloud.assets/image-20220701171329457.png)

## Docker

![image-20220701172443000](SpringCloud.assets/image-20220701172443000.png)

![image-20220701172927681](SpringCloud.assets/image-20220701172927681.png)

![image-20220701173251562](SpringCloud.assets/image-20220701173251562.png)



![image-20220701190940385](SpringCloud.assets/image-20220701190940385.png)

![image-20220701200338964](SpringCloud.assets/image-20220701200338964.png)

![image-20220701201340502](SpringCloud.assets/image-20220701201340502.png)

![image-20220701201348431](SpringCloud.assets/image-20220701201348431.png)

![image-20220701210214590](SpringCloud.assets/image-20220701210214590.png)

![image-20220701225103671](SpringCloud.assets/image-20220701225103671.png)

![image-20220701225429710](SpringCloud.assets/image-20220701225429710.png)