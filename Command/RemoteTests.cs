using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace Command
{
    [TestFixture]
    public class RemoteTests
    {
        [Test]
        public void OneButtonRemoteLightOnTest()
        {
            OneButtonRemote remote = new OneButtonRemote();
            ILight light = MockRepository.GenerateMock<ILight>();
            LightOnCommand command = new LightOnCommand(light);
            remote.SetCommand(command);
            remote.PushButton();
            light.AssertWasCalled(x => x.On());
        }

        [Test]
        public void OneButtonRemoteLightOffTest()
        {
            OneButtonRemote remote = new OneButtonRemote();
            ILight light = MockRepository.GenerateMock<ILight>();
            LightOffCommand command = new LightOffCommand(light);
            remote.SetCommand(command);
            remote.PushButton();
            light.AssertWasCalled(x => x.Off());
        }

        [Test]
        public void LightOnCommandTest()
        {
            ILight light = MockRepository.GenerateMock<ILight>();
            LightOnCommand command = new LightOnCommand(light);
            command.Execute();
            light.AssertWasCalled(x => x.On());
        }

        [Test]
        public void MultiButtonRemoteWithMultipleLightsTest()
        {
            var kitchenLight = MockRepository.GenerateMock<ILight>();
            var kitchenLightOnCommand = new LightOnCommand(kitchenLight);
            var kitchenLightOffCommand = new LightOffCommand(kitchenLight);
            var livingRoomLight = MockRepository.GenerateMock<ILight>();
            var livingRoomLightOnCommand = new LightOnCommand(livingRoomLight);
            var livingRoomLightOffCommand = new LightOffCommand(livingRoomLight);
            
            MultiButtonRemote remote = new MultiButtonRemote();
            remote.SetSlot(0, kitchenLightOnCommand, kitchenLightOffCommand);
            remote.SetSlot(1, livingRoomLightOnCommand, livingRoomLightOffCommand);

            remote.PushOnButton(0);
            kitchenLight.AssertWasCalled(x => x.On());
            remote.PushOnButton(1);
            livingRoomLight.AssertWasCalled(x => x.On());

            remote.PushOffButton(0);
            kitchenLight.AssertWasCalled(x => x.Off());
            remote.PushOffButton(1);
            kitchenLight.AssertWasCalled(x => x.Off());
        }
    }

    public class MultiButtonRemote
    {
        private ICommand[] onCommands = new ICommand[7];
        private ICommand[] offCommands = new ICommand[7];

        public MultiButtonRemote()
        {
            ICommand noCommand = new NoCommand();
            for (int i = 0; i < 7; i++)
            {
                onCommands[i] = noCommand;
                offCommands[i] = noCommand;
            }
        }
        public void SetSlot(int slotNumber, ICommand onCommand, ICommand offCommand)
        {
            throw new NotImplementedException();
        }

        public void PushOnButton(int slotNumber)
        {
            throw new NotImplementedException();
        }

        public void PushOffButton(int slotNumber)
        {
            throw new NotImplementedException();
        }
    }

    public class NoCommand : ICommand
    {
        public void Execute()
        {
            // intentionally left blank
        }
    }

    public class LightOffCommand : ICommand
    {
        private readonly ILight light;

        public LightOffCommand(ILight light)
        {
            this.light = light;
        }

        public void Execute()
        {
            light.Off();
        }
    }

    public interface ICommand
    {
        void Execute();
    }

    public class LightOnCommand : ICommand
    {
        private readonly ILight light;

        public LightOnCommand(ILight light)
        {
            this.light = light;
        }

        public void Execute()
        {
            light.On();
        }
    }

    public interface ILight
    {
        void On();
        void Off();
    }

    public class OneButtonRemote
    {
        private ICommand command;

        public void SetCommand(ICommand command)
        {
            this.command = command;
        }

        public void PushButton()
        {
            command.Execute();
        }
    }
}
