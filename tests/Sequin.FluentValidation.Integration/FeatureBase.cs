namespace Sequin.FluentValidation.Integration
{
    using System.Reflection;
    using Infrastructure;
    using Microsoft.Owin.Testing;
    using Middleware;
    using Xbehave;

    public abstract class FeatureBase
    {
        private CommandTrackingPostProcessor postProcessor;

        protected TestServer Server { get; private set; }

        [Background]
        public void Background()
        {
            postProcessor = new CommandTrackingPostProcessor();
            Server = TestServer.Create(app =>
                                       {
                                           app.UseSequin(new SequinOptions
                                                         {
                                                            PostProcessor = postProcessor,
                                                            CommandPipeline = new []
                                                                              {
                                                                                  new CommandPipelineStage(typeof(FluentValidateCommand), new ReflectionValidatorFactory(Assembly.GetExecutingAssembly())), 
                                                                              }
                                                         });
                                       });
        }

        protected bool HasExecuted(string commandName)
        {
            return postProcessor.ExecutedCommands.ContainsKey(commandName);
        }
    }
}