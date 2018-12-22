using dev.Core.Entities;
using dev.Core.IoC;
using dev.Core.Logger;
using dev.Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace dev.Core.Commands
{
    public class Handler : IHandler
    {
        private ILog _log;
        private readonly IServiceLocator _serviceLocator;

        public List<IModel> Data { get; set; }
        public List<ICommand> Commands { get; set; }
        public List<IValidation> Validators { get; set; }

        public Handler(IServiceLocator locator)
        {
            Data = new List<IModel>();
            Commands = new List<ICommand>();
            Validators = new List<IValidation>();

            _serviceLocator = locator;
            _log = _serviceLocator.Resolve<ILog>();
        }

        private IResult Validate()
        {
            var result = new Result();

            foreach (var validator in Validators)
            {
                if (validator.IsValid(Data))
                    continue;
                result.AddMessage(validator.Message());
                result.Success = false;
            }

            return result;
        }

        private IResult Run()
        {
            var handler = new Stopwatch();
            var rule = new Stopwatch();

            try
            {
                handler.Start();

                _log.LogTrace<Handler>($"Handler Start: [{Commands.Count}] Commands Queued.");

                Commands.ForEach(c => {

                    rule.Reset();
                    rule.Start();

                    c.Execute(Data);

                    rule.Stop();

                    _log.LogTrace(c, $"Command: {c.GetType().Name} Processed in [{rule.Elapsed.ToString(@"hh\:mm\:ss\:fff")}]");
                });

            }
            catch (Exception ex)
            {
                _log.LogException<Handler>(ex);

                var result = new Result()
                {
                    Success = false
                };
                result.AddMessage($"Looks like there was a problem with your request.");
                return result;
            }
            finally
            {
                handler.Stop();

                _log.LogTrace<Handler>($"Handler Complete: [{handler.Elapsed.ToString(@"hh\:mm\:ss\:fff")}]");
            }

            return new Result() { Success = true, Data = Data };
        }

        public IHandler Add(IModel data)
        {
            Data.Add(data);

            return this;
        }

        public IHandler Command<Command>() where Command : ICommand
        {
            Commands.Add(_serviceLocator.Resolve<ICommand>(typeof(Command).Name));

            return this;
        }

        public IHandler Validate<Validator>() where Validator : IValidation
        {
            Validators.Add(_serviceLocator.Resolve<IValidation>(typeof(Validator).Name));

            return this;
        }

        public IResult Invoke()
        {
            IResult result = new Result();

            result = Validate();
            if (!result.Success)
                return result;

            //todo: add rollback command
            result = Run();
            if (!result.Success)
                return result;

            return result;
        }
    }
}