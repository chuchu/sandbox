package main

import (
	context "context"
	"log"
	"net"

	grpc "google.golang.org/grpc"
)

type server struct {
	UnimplementedTriggerServer
	name string
}

func (s *server) FetchResource(
	ctx context.Context,
	in *FetchResourceRequest) (*FetchResourceResponse, error) {

	log.Print("FetchResource was called.")
	log.Printf("Resource: %s", string(in.Resource))

	// resp, err := http.Get("https://raw.githubusercontent.com/VaibhavPage/tekton-cd-trigger/master/example.yaml")
	// if err != nil {
	// 	log.Fatalln(err)
	// }

	// body, err := ioutil.ReadAll(resp.Body)
	// if err != nil {
	// 	log.Fatalln(err)
	// }

	response := FetchResourceResponse{
		Resource: []byte("{}"),
	}
	return &response, nil
}

func (s *server) Execute(
	context.Context,
	*ExecuteRequest) (*ExecuteResponse, error) {

	log.Print("Execute was called.")
	response := ExecuteResponse{
		Response: []byte(""),
	}
	return &response, nil
}

func (s *server) ApplyPolicy(
	context.Context,
	*ApplyPolicyRequest) (*ApplyPolicyResponse, error) {

	log.Print("ApplyPolicy was called.")

	log.Print("Execute was called.")
	response := ApplyPolicyResponse{
		Success: true,
		Message: "",
	}
	return &response, nil
}

func main() {
	listener, err := net.Listen("tcp", "127.0.0.1:8081")

	if err != nil {
		log.Fatalf("failed to listen: %v", err)
	}

	grpcServer := grpc.NewServer()

	triggerServer := server{
		name: "CustomEventSource",
	}

	RegisterTriggerServer(grpcServer, &triggerServer)

	log.Printf("Server listening at %v", listener.Addr())

	if err := grpcServer.Serve(listener); err != nil {
		log.Fatalf("failed to serve: %v", err)
	}
}
