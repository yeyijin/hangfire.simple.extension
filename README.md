# 文档
- 中文文档：https://docs.hangfire.io/en/latest/background-methods/index.html#states
- 官方文档：https://docs.hangfire.io/en/latest/background-processing/configuring-queues.html


# 使用
- 设置配置HangfireConfig

- ConfigureServices 配置依赖注入
`service.AddHangfireServiceConfig`

- Configure中开启可视化面板
```
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });

            app.UseHangfireDashboard();
```



# 设置错误重试次数
```
[AutomaticRetry(Attempts = 0)]
public void BackgroundMethod()
{
}

```


# 设置任务的工作队列
```
[Queue("alpha")]
public void SomeMethod() { }
```

# 多种方式注册服务,另一种也可以在Configure中注册
```
            var options = new BackgroundJobServerOptions { WorkerCount = Environment.ProcessorCount * 5};
            app.UseHangfireServer(options);

```
