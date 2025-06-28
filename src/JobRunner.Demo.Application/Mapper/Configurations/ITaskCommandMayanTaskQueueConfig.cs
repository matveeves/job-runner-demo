using JobRunner.Demo.Application.Interfaces;
using JobRunner.Demo.Application.Models;
using JobRunner.Demo.Domain.Entities;
using Mapster;

namespace JobRunner.Demo.Application.Mapper.Configurations;

public class TaskCommandMayanTaskQueueConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TaskQueueItem, ITaskCommand>()
            .Map(dest => dest.MaxTries, src => src.TaskSchedule!.MaxTries)
            .Map(dest => dest.ExceptionsJson, src => src.JError);

        config.NewConfig<ITaskCommand, TaskException>()
            .Map(dest => dest.Task, src => src);

        config.NewConfig<Exception, TaskException>()
            .Map(dest => dest.Exception.ExceptionType, src => src.GetType().Name)
            .Map(dest => dest.Exception, src => src)
            .AfterMapping((src, dest) =>
            {
                dest.Exception.Metadata = new Metadata
                {
                    Assembly = src.TargetSite?.DeclaringType?.Assembly.FullName,
                    Method = src.TargetSite?.Name
                };
            });

        config.NewConfig<Exception, InnerException>()
            .Map(dest => dest.ExceptionType, src => src.GetType().Name);
    }
}
