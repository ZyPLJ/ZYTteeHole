# ZY树洞
# 前言
ZY树洞是一个基于.NET Core开发的简单的评论系统，主要用于大家分享自己心中的感悟、经验、心得、想法等。  
好了，不卖关子了，这个项目其实是上班无聊的时候写的，为什么要写这个项目呢？因为我单纯的想吐槽一下工作中的不满而已。

# 项目介绍
项目很简单，主要功能就是提供一个简单的评论系统，用户可以发布自己的评论，然后评论以弹幕的形式展示在页面上。  
项目前端页面地址：
- [ZyPLJ/TreeHoleVue (github.com)](https://github.com/ZyPLJ/TreeHoleVue) https://github.com/ZyPLJ/TreeHoleVue

目前项目测试访问地址：
- http://tree.pljzy.top/ 注意是http,输出https就访问到博客里面去了。


# 项目特点
- 基于.NET Core开发，跨平台
- 简单易用，界面简洁
- 匿名评论，不用注册即可发表评论
- 弹幕效果，评论以弹幕的形式展示在页面上

# 主要技术
- ASP.NET Core Web API
- Entity Framework Core
- Sql Server数据库 | Sqlite3数据库
- Vue.js
# 项目结构
- ZYTreeHole 主项目
- ZYTreeHole.Tests 集成测试
- ZYTreeHole_Services 服务层
- ZYTreeHole_Models 模型层
# 项目截图
![1.png](https://www.pljzy.top/media/photofraphy/1dd68d7f25886dac.jpg)
# 后端部署
创建数据库，默认用的是sqlite3数据库，如需更改要自行替换连接字符串。连接字符串分为2部分，`appsetting.json`中的用于项目访问数据库，而`Models`中的`MyDbContextDesignFac`类用于CodeFirst模式生成数据库。
如果不更换数据库则不需要更改
直接使用终端，进入**ZYTreeHole_Models**目录输入：
``` C#
dotnet ef migrations add Init //生成迁移文件
dotnet ef database update //更新数据库
```
使用dotnet语句需安装`.NET SDK`
- [下载 .NET(Linux、macOS 和 Windows) (microsoft.com)](https://dotnet.microsoft.com/zh-cn/download)


完成数据库生成后，会在Models层生成一个app.db文件，将该文件剪切到ZYTreeHole 主项目里面去就行了。完成上述步骤即可开始部署，将项目打包成文件夹形式，这里就不多讲了。
我是采用的Docker部署，DockerFile文件项目已经包含进去了。
在打包后端路径中打开终端执行，默认部署为44323端口。
``` sh
docker build -t treehole . --下载镜像
docker run -d -p 44323:44323 -v /...替换成你的打包路径/treehole:/src --name treehole treehole --创建容器
```
# Docker镜像无法下载问题解决
相关链接：[国内镜像源下架的解决办法-米续硬 (mixuying.com)](https://mixuying.com/archives/1719753069678)

# 待完成的点
- [x] 评论限流
- [x] 关键词过滤

- [x] 将前端弹幕设置滚动频率、速度等写入配置文件或者数据库。
- [ ] 完成后台管理模块的编写。
- [x] 前端页面美化

欢迎各位提提意见~.~
# 参考链接
- 前端弹幕效果使用开源项目：[https://github.com/hellodigua/vue-danmaku](https://github.com/hellodigua/vue-danmaku)
- 接口限流：[.NET Core WebApi接口ip限流实践 - 妙妙屋（zy） - 博客园 (cnblogs.com)](https://www.cnblogs.com/ZYPLJ/p/17243389.html)
- 评论关键词过滤：[基于.NetCore开发博客项目 StarBlog - (30) 实现评论系统 - 程序设计实验室 - 博客园 (cnblogs.com)](https://www.cnblogs.com/deali/p/17910094.html#%E5%B0%8F%E7%AE%A1%E5%AE%B6%E5%AE%A1%E6%A0%B8)