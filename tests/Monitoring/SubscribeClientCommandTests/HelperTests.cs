using NUnit.Framework;
using MNX.MonitoringCenter.Monitoring.Contracts.Models;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using Moq;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Tests;

[TestFixture]
public class HelperTests
{
    [Test]
    public async Task HelperTest_ForRigsDynamicData()
    {
        // Arrange
        List<RigDynamicData?> rigsDynamicData = new()
        {
            // Ожидаемый индекс 1
            new RigDynamicData(){
                Id = Guid.Parse("ab913901-aab9-453c-b199-1afbcabd3304"),
            },

            // Ожидаемый индекс 2
            new RigDynamicData(){
                Id = Guid.Parse("219a73e6-8060-4054-b21a-d2b43a2756fc"),
            },

            // Ожидаемый индекс 4
            new RigDynamicData(){
                Id = Guid.Parse("359f50b3-46cc-45cb-bcbe-daa1ade2ac5f"),
            },

            // Ожидаемый индекс 6
            new RigDynamicData(){
                Id = Guid.Parse("4e12cbf5-1a0b-464e-bf57-d3d1b5666ffa"),
            },
        };

        var rigsFromRepository = GetAvailableRigs(1);
        var rigs = await rigsFromRepository.ToListAsync();
        var repositoryRigs = rigs.ToList();

        var mappedRigsData = await Helper.MapDbDataToRigDataAsync(rigsFromRepository, rigsDynamicData);


        // Assert    
        Assert.Multiple(() =>
        {
            // Первый элемент
            Assert.That(mappedRigsData[0]?.Id, Is.EqualTo(repositoryRigs[0].Id));
            Assert.That(mappedRigsData[0]?.Index, Is.EqualTo(1));

            // Второй элемент
            Assert.That(mappedRigsData[1]?.Id, Is.EqualTo(repositoryRigs[1].Id));
            Assert.That(mappedRigsData[1]?.Index, Is.EqualTo(2));

            Assert.That(mappedRigsData[2], Is.Null);

            // Третий элемент
            Assert.That(mappedRigsData[3]?.Id, Is.EqualTo(repositoryRigs[3].Id));
            Assert.That(mappedRigsData[3]?.Index, Is.EqualTo(4));

            Assert.That(mappedRigsData[4], Is.Null);

            // Четвертый элемент
            Assert.That(mappedRigsData[5]?.Id, Is.EqualTo(repositoryRigs[5].Id));
            Assert.That(mappedRigsData[5]?.Index, Is.EqualTo(6));
        });
    }

    [Test]
    public async Task HelperTest_ForRigsState()
    {
        List<RigState?> rigsState = new()
        {
            // Ожидаемый индекс в списке 1
            new RigState(){
                Id = Guid.Parse("219a73e6-8060-4054-b21a-d2b43a2756fc"),
            },

            // Ожидаемый индекс в списке 2
            new RigState(){
                Id = Guid.Parse("03081951-29a0-42fb-99b2-e27325701bae"),
            },

            // Ожидаемый индекс в списке 5
            new RigState(){
                Id = Guid.Parse("4e12cbf5-1a0b-464e-bf57-d3d1b5666ffa"),
            },
        };


        var rigsFromRepository = GetAvailableRigs(1);
        var rigs = await rigsFromRepository.ToListAsync();
        var repositoryRigs = rigs.ToList();

        var mappedRigsState = await Helper.MapDbDataToRigDataAsync(rigsFromRepository, rigsState);

        // Assert    
        Assert.Multiple(() =>
        {
            Assert.That(mappedRigsState[0], Is.Null);

            // Второй элемент
            Assert.That(mappedRigsState[1]?.Id, Is.EqualTo(repositoryRigs[1].Id));

            // Третий элемент
            Assert.That(mappedRigsState[2]?.Id, Is.EqualTo(repositoryRigs[2].Id));

            Assert.That(mappedRigsState[3], Is.Null);

            Assert.That(mappedRigsState[4], Is.Null);

            // Пятый элемент
            Assert.That(mappedRigsState[5]?.Id, Is.EqualTo(repositoryRigs[5].Id));
        });
    }

    private static async IAsyncEnumerable<Rig> GetAvailableRigs(long userId)
    {
        List<Rig> rigsList = new()
        {
            // 1
            new Rig(){
                Id = Guid.Parse("ab913901-aab9-453c-b199-1afbcabd3304"),
                UserId = userId,
            },

            // 2
            new Rig(){
                Id = Guid.Parse("219a73e6-8060-4054-b21a-d2b43a2756fc"),
                UserId = userId
            },

            // 3
            new Rig(){
                Id = Guid.Parse("03081951-29a0-42fb-99b2-e27325701bae"),
                UserId = userId
            },

            // 4
            new Rig(){
                Id = Guid.Parse("359f50b3-46cc-45cb-bcbe-daa1ade2ac5f"),
                UserId = userId
            },

            // 5
            new Rig(){
                Id = Guid.Parse("ed9b58d8-7b59-4932-a89f-db421f980d99"),
                UserId = userId
            },

            // 6
            new Rig(){
                Id = Guid.Parse("4e12cbf5-1a0b-464e-bf57-d3d1b5666ffa"),
                UserId = userId
            },
        };

        var rigRepository = new Mock<IRigRepository>();
        rigRepository.Setup(x => x.GetAvailable(userId)).Returns(rigsList.ToAsyncEnumerable());

        await foreach (var rig in rigsList.ToAsyncEnumerable())
        {
            yield return rig;
        }
    }
}
