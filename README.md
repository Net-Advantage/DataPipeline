# Nabs Data Pipeline

This is a demonstration of an opinionated data pipeline.

The following goals are to be achieved:

- [x] Build an extensible and flexible data pipeline service.
- [ ] Each pipeline must be able to easily be configured by developers.
- [ ] Each pipeline must be able to contain multiple processing stages.
- [ ] The stages should be able to be individually defined and even made to be reusable.
- [ ] Each stage must be able to contain multiple steps.
- [ ] Each step should be able to be individually defined and even made to be reusable.

Here is the structure of the Nabs Data Pipeline:

```
Events
Connection
PipelineOrchestrator
Pipeline
  - Stage
    - Step

```
