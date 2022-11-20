
Export the port so that it can be reached:

```bash
kubectl --namespace argo-events port-forward [NAME_OF_THE_POD] 12000:12000 &
```

Send a message:


```bash
curl -X POST -H "Content-Type: application/json" -d '{"message":"MyMessage"}' http://localhost:12000/devops-toolkit
```
