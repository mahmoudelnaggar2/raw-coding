// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using LOHvsSOH;

var stats = BenchmarkRunner.Run<Allocation>();
// var stats = BenchmarkRunner.Run<Access>();
// var stats = BenchmarkRunner.Run<LoopOver>();


