# DDD洋葱架构的基础构成

## Domain:实体类，时间，防腐层接口，仓储接口，领域服务

## Infrastructure(基础设施): 实体类的配置，DbContext，防腐层接口实现，仓储接口实现

## WebApi: Controller,事件(领域事件，集成事件)的监听响应类

=====================================================

在洋葱架构中进行数据迁移时，首先进入数据层(防腐层)文件内，然后使用：

`dotnet ef --startup-project ../UserManager.WebAPI/ migrations add init`

`dotnet ef --startup-project ../UserManager.WebAPI/ database update`

进行迁移和更新数据库
