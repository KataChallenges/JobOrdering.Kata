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
        public void Test_no_jobs_supplied_returns_empty()
        {
            Assert.Equal("", _jobOrderService.OrderJobs(""));
        }

        [Fact]
        public void Test_single_job_gives_one_output()
        {
            Assert.Equal("a", _jobOrderService.OrderJobs("a =>"));
        }

        [Fact]
        public void Test_multiple_jobs_gives_jobs_in_given_order()
        {
            Assert.Equal("abc", _jobOrderService.OrderJobs("a =>\n b =>\n c =>"));
        }

        [Fact]
        public void Test_multiple_jobs_with_dependency_use_dependency_first()
        {
            const string JOBS =
                "a =>\n" +
                "b => c\n" +
                "c =>";
            Assert.Equal("acb", _jobOrderService.OrderJobs(JOBS));
        }

        [Fact]
        public void Test_multiple_jobs_with_dependencies_use_dependencies_first()
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
        public void Test_self_referencing_jobs_are_not_allowed()
        {
            const string JOBS =
                "a =>\n" +
                "b =>\n" +
                "c => c";
            Assert.Throws<Exception>(() => _jobOrderService.OrderJobs(JOBS));
        }

        [Fact]
        public void Test_circular_dependencies_are_not_allowed()
        {
            const string JOBS =
                "a =>\n" +
                "b => c\n" +
                "c => f\n" +
                "d => a\n" +
                "e =>\n" +
                "f => b";
            Assert.Throws<Exception>(() => _jobOrderService.OrderJobs(JOBS));
        }
    }
}
