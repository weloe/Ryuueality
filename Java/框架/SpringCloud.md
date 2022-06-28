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