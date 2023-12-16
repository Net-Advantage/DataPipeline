﻿namespace Nabs.DataPipeline.Steps;

/// <summary>
/// Parse an XML string and outputs an XDocument.
/// </summary>
/// <typeparam name="TStage"></typeparam>
/// <param name="stage"></param>
public sealed class XmlToStringExtractionStep<TStage>(TStage stage)
	: Step<TStage, XDocument>(stage)
		where TStage : class, IStage, IXmlExtractionStepInput<string>
		
{
	public override async Task Transform()
	{
		var sourceFile = await Stage.SourceConnection.Extract();

		if(string.IsNullOrWhiteSpace(sourceFile))
		{
			StepOutput = new XDocument();
			return;
		}

		StepOutput = XDocument.Parse(sourceFile);
	}
}


public interface IXmlExtractionStepInput<T>
{
	ISourceConnection<T> SourceConnection { get; }
}