using System;
using Xunit;

namespace OTB.JobOrdering.Kata.Application.Tests.Unit
{
    public class JobOrderingTests
    {
        private readonly IJobOrderService _jobOrderService;

        public JobOrderingTests()
        {
            _jobOrderService = new JobOrderService();
        }


        [Fact]
        public void Test_when_no_jobs_supplied_Returns_Empty()
        {
            Assert.Equal("", _jobOrderService.OrderJobs(""));
        }

        [Theory]
        [InlineData("a", "a =>")]
        [InlineData("b", "b =>")]
        [InlineData("c", "c =>")]
        [InlineData("d", "d =>")]
        public void Test_When_Single_job_gives_one_output(string result, string jobId)
        {
            Assert.Equal(result, _jobOrderService.OrderJobs(jobId));
        }

        [Theory]
        [InlineData("abc", "a =>\n b =>\n c =>")]
        [InlineData("bcd", "b =>\n c =>\n d =>")]
        [InlineData("def", "d =>\n e =>\n f =>")]
        [InlineData("cba", "c =>\n b =>\n a =>")]
        public void Test_When_Multiple_jobs_gives_jobs_in_given_order(string result, string jobIds)
        {
            Assert.Equal(result, _jobOrderService.OrderJobs(jobIds));
        }




        [Fact]
        public void Test_When_Multiple_jobs_with_dependency_use_dependency_first()
        {
            const string JOBS =
                "a =>\n" +
                "b => c\n" +
                "c =>";
            Assert.Equal("acb", _jobOrderService.OrderJobs(JOBS));
        }

        [Fact]
        public void Test_When_Multiple_jobs_with_dependencies_use_dependencies_first()
        {
            const string JOBS =
                "a =>\n" +
                "b => c\n" +
                "c => f\n" +
                "d => a\n" +
                "e => b\n" +
                "f =>";
            Assert.Equal("afcbde", _jobOrderService.OrderJobs(JOBS));
        }

        [Fact]
        public void Test_When_Self_referencing_jobs_are_not_allowed()
        {
            const string JOBS =
                "a =>\n" +
                "b =>\n" +
                "c => c";
            Assert.Throws<InvalidOperationException>(() => _jobOrderService.OrderJobs(JOBS));
        }

        [Fact]
        public void Test_When_Circular_dependencies_are_not_allowed()
        {
            const string JOBS =
                "a =>\n" +
                "b => c\n" +
                "c => f\n" +
                "d => a\n" +
                "e =>\n" +
                "f => b";
            Assert.Throws<InvalidOperationException>(() => _jobOrderService.OrderJobs(JOBS));
        }
    }
}
