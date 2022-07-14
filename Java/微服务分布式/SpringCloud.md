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

![image-20220703010816132](SpringCloud.assets/image-20220703010816132.png)

```console
 docker run \
  --name some-mysql \
  -e MYSQL_ROOT_PASSWORD=123 \
  -p 3306:3306 \
  -v /tmp/mysql/conf/hmy.cnf:/etc/mysql/conf.d/hmy.cn \
  -v /tmp/mysql/data:/var/lib/mysql \
  -d  \
  mysql:5.7.25
```

![image-20220703015556462](SpringCloud.assets/image-20220703015556462.png)

### 自定义镜像

![image-20220703015651188](SpringCloud.assets/image-20220703015651188.png)

## MQ

![image-20220703163249634](SpringCloud.assets/image-20220703163249634.png)

![image-20220704140841841](SpringCloud.assets/image-20220704140841841.png)

![image-20220704141750664](SpringCloud.assets/image-20220704141750664.png)

![image-20220704145304588](SpringCloud.assets/image-20220704145304588.png)

![image-20220704151658808](SpringCloud.assets/image-20220704151658808.png)

![image-20220704194158714](SpringCloud.assets/image-20220704194158714.png)

![image-20220704203655508](SpringCloud.assets/image-20220704203655508.png)

![image-20220704212209003](SpringCloud.assets/image-20220704212209003.png)

![image-20220704215558302](SpringCloud.assets/image-20220704215558302.png)

## ES

![image-20220704220750683](SpringCloud.assets/image-20220704220750683.png)

![image-20220705024812022](SpringCloud.assets/image-20220705024812022.png)

![image-20220705124504130](SpringCloud.assets/image-20220705124504130.png)

![image-20220705141154872](SpringCloud.assets/image-20220705141154872.png)

![image-20220705144218353](SpringCloud.assets/image-20220705144218353.png)

![image-20220705150028303](SpringCloud.assets/image-20220705150028303.png)



![image-20220705154511517](SpringCloud.assets/image-20220705154511517.png)

### RestClient操作索引库，文档 

![image-20220705183717044](SpringCloud.assets/image-20220705183717044.png)

![image-20220705210850836](SpringCloud.assets/image-20220705210850836.png)

### match查询

![image-20220708175835211](SpringCloud.assets/image-20220708175835211.png)

![image-20220709002216906](SpringCloud.assets/image-20220709002216906.png)

```D
GET /hotel/_search
{
  "query":{	
    "geo_distance":{
      "distance":"15km",
      "location":"31.21,121.5"
    }
  }
}
```

### 相关性算分

![image-20220709012729992](SpringCloud.assets/image-20220709012729992.png)

![image-20220709020219685](SpringCloud.assets/image-20220709020219685.png)

```D
GET /hotel/_search
{
  "query": {
    "function_score": {
      "query":{
        "match": {
        "name": "外滩"
      }},
      "functions": [
        {
          "filter": {
            "term": {
              "brand": "如家"
            }
          },
          "weight": 10
          
        }
      ],
      "boost_mode": "multiply"
    }
  }
}
```

### 复合查询

![image-20220712225733262](SpringCloud.assets/image-20220712225733262.png)

![image-20220712230652687](SpringCloud.assets/image-20220712230652687.png)

### 搜索结果处理

#### 排序

![image-20220712232148473](SpringCloud.assets/image-20220712232148473.png)

#### 分页![image-20220713000755545](SpringCloud.assets/image-20220713000755545.png)

![image-20220713140538388](SpringCloud.assets/image-20220713140538388.png)

![image-20220713140901375](SpringCloud.assets/image-20220713140901375.png)

![image-20220713141018441](SpringCloud.assets/image-20220713141018441.png)

#### 高亮

![image-20220713142310312](SpringCloud.assets/image-20220713142310312.png)

默认清空下ES搜索字段必须与高亮字段一致，否则不会高亮

`"require_field_match": "false"`

```json
GET /hotel/_search
{
  "query": {
    "match": {
      "all": "如家"
    }
  },
  "highlight": {
    "fields": {
      "name": {
        "require_field_match": "false"
      }
    }
  }
}
```

![image-20220713142720344](SpringCloud.assets/image-20220713142720344.png)

### RestClient查询文档

#### 查询

![image-20220713143101646](SpringCloud.assets/image-20220713143101646.png)

![image-20220713145603773](SpringCloud.assets/image-20220713145603773.png)

![image-20220713150313537](SpringCloud.assets/image-20220713150313537.png)

![image-20220713150323081](SpringCloud.assets/image-20220713150323081.png)

![image-20220713150404254](SpringCloud.assets/image-20220713150404254.png)

![image-20220713150919142](SpringCloud.assets/image-20220713150919142.png)

![image-20220713151358935](SpringCloud.assets/image-20220713151358935.png)

![image-20220713151534720](SpringCloud.assets/image-20220713151534720.png)

**要构建查询条件，关键在于：QueryBuilders**

#### 排序分页

![image-20220713152244111](SpringCloud.assets/image-20220713152244111.png)

#### 高亮

![image-20220713153759547](SpringCloud.assets/image-20220713153759547.png)

![image-20220713154422299](SpringCloud.assets/image-20220713154422299.png)

![image-20220713154929464](SpringCloud.assets/image-20220713154929464.png)

#### 距离排序

![image-20220713170419926](SpringCloud.assets/image-20220713170419926.png)

#### 组合查询

![image-20220713172448056](SpringCloud.assets/image-20220713172448056.png)

### 聚合

![image-20220713174307722](SpringCloud.assets/image-20220713174307722.png)

![image-20220713174420290](SpringCloud.assets/image-20220713174420290.png)

![image-20220713175115248](SpringCloud.assets/image-20220713175115248.png)

![image-20220713180628758](SpringCloud.assets/image-20220713180628758.png)

![image-20220713180755520](SpringCloud.assets/image-20220713180755520.png)

![image-20220713181012012](SpringCloud.assets/image-20220713181012012.png)

![image-20220713181219880](SpringCloud.assets/image-20220713181219880.png)

```json
GET /hotel/_search
{
  "size": 0,
  "aggs": {
    " brandAgg": {
      "terms": {
        "field": "brand",
        "size": 10,
        "order": {
          "_count": "asc"
        }
      }
    }
  }
}

GET /hotel/_search
{
  "query": {
    "range": {
      "price": {
        "lte": 200
      }
    }
  }, 
  "size": 0,
  "aggs": {
    " brandAgg": {
      "terms": {
        "field": "brand",
        "size": 10,
        "order": {
          "_count": "asc"
        }
      }
    }
  }
}

GET /hotel/_search
{
  "size": 0,
  "aggs": {
    " brandAgg": {
      "terms": {
        "field": "brand",
        "size": 20,
        "order": {
          "scoreAgg.avg": "desc"
        }
      },
      "aggs": {
        "scoreAgg": {
          "stats": {
            "field": "score"
          }
        }
      }
    }
  }
}
```

![image-20220713191425768](SpringCloud.assets/image-20220713191425768.png)

![image-20220713192122494](SpringCloud.assets/image-20220713192122494.png)

上传拼音分词器

![image-20220713201921885](SpringCloud.assets/image-20220713201921885.png)

![image-20220713202309156](SpringCloud.assets/image-20220713202309156.png)

![image-20220713203414121](SpringCloud.assets/image-20220713203414121.png)

```json
PUT /test
{
  "settings": {
    "analysis": {
      "analyzer": { 
        "my_analyzer": { 
          "tokenizer": "ik_max_word",
          "filter": "py"
        }
      },
      "filter": {
        "py": { 
          "type": "pinyin",
          "keep_full_pinyin": false,
          "keep_joined_full_pinyin": true,
          "keep_original": true,
          "limit_first_letter_length": 16,
          "remove_duplicated_term": true,
          "none_chinese_pinyin_tokenize": false
        }
      }
    }
  }
}
```

![image-20220713204959690](SpringCloud.assets/image-20220713204959690.png)

![image-20220713205221127](SpringCloud.assets/image-20220713205221127.png)

![image-20220713205540161](SpringCloud.assets/image-20220713205540161.png)

![image-20220713210440192](SpringCloud.assets/image-20220713210440192.png)

![image-20220713210610603](SpringCloud.assets/image-20220713210610603.png)

![image-20220713213334099](SpringCloud.assets/image-20220713213334099.png)

```json
PUT /hotel
{
  "settings": {
    "analysis": {
      "analyzer": {
        "text_anlyzer": {
          "tokenizer": "ik_max_word",
          "filter": "py"
        },
        "completion_analyzer": {
          "tokenizer": "keyword",
          "filter": "py"
        }
      },
      "filter": {
        "py": {
          "type": "pinyin",
          "keep_full_pinyin": false,
          "keep_joined_full_pinyin": true,
          "keep_original": true,
          "limit_first_letter_length": 16,
          "remove_duplicated_term": true,
          "none_chinese_pinyin_tokenize": false
        }
      }
    }
  },
  "mappings": {
    "properties": {
      "id":{
        "type": "keyword"
      },
      "name":{
        "type": "text",
        "analyzer": "text_anlyzer",
        "search_analyzer": "ik_smart",
        "copy_to": "all"
      },
      "address":{
        "type": "keyword",
        "index": false
      },
      "price":{
        "type": "integer"
      },
      "score":{
        "type": "integer"
      },
      "brand":{
        "type": "keyword",
        "copy_to": "all"
      },
      "city":{
        "type": "keyword"
      },
      "starName":{
        "type": "keyword"
      },
      "business":{
        "type": "keyword",
        "copy_to": "all"
      },
      "location":{
        "type": "geo_point"
      },
      "pic":{
        "type": "keyword",
        "index": false
      },
      "all":{
        "type": "text",
        "analyzer": "text_anlyzer",
        "search_analyzer": "ik_smart"
      },
      "suggestion":{
          "type": "completion",
          "analyzer": "completion_analyzer"
      }
    }
  }
}
```

![image-20220713215639826](SpringCloud.assets/image-20220713215639826.png)

![image-20220713220332736](SpringCloud.assets/image-20220713220332736.png)

### 数据同步

![image-20220713224212345](SpringCloud.assets/image-20220713224212345.png)

![image-20220713224443640](SpringCloud.assets/image-20220713224443640.png)

有业务耦合，性能下降，hotel-demo会影响hotel-admin的操作

![image-20220713224941068](SpringCloud.assets/image-20220713224941068.png)

依赖mq的可靠性

![image-20220713225017525](SpringCloud.assets/image-20220713225017525.png)

canal监听mysql的binlog，无耦合

![image-20220713225241269](SpringCloud.assets/image-20220713225241269.png)

#### MQ实现mysql与elasticsearch数据同步

![image-20220713233514489](SpringCloud.assets/image-20220713233514489.png)

### ES集群

![image-20220714135101716](SpringCloud.assets/image-20220714135101716.png)

docker部署es集群

cerebro管理集群

![image-20220714145158122](SpringCloud.assets/image-20220714145158122.png)
