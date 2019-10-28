# NLogUsage
NLog在asp.net网站中的使用，NLog日志写入数据库，NLog日志写入文件

实现了：  
1.日志自动写入到数据库、写入到文件  
2.appsettings.json数据库连接更改后，不需要去改NLog中的连接地址，启动网站或项目时自动检测变动然后去更改，以appsettings.json为准，保持同步。  
3.写入日志时，除了NLog自带的字段，新增LogType自定义字段记录日志类型，例如网站日志、中间件日志等  
4.统一的写日志方法，不用每次get一个logger对象（或依赖注入）来记日志  

