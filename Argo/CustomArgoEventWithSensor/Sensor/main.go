package main

import (
	context "context"
	"errors"
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

	// return nil, errors.New("Out of resources.")

	response := FetchResourceResponse{
		Resource: []byte("{\"key\": \"value\"}"),
	}

	return &response, nil
}

func (s *server) Execute(
	ctx context.Context,
	in *ExecuteRequest) (*ExecuteResponse, error) {

	log.Print("Execute was called.")
	log.Printf("Payload: %s", in.Payload)
	log.Printf("Resource: %s", in.Resource)

	log.Print("Return error.")
	return nil, errors.New("Out of resources.")

	// response := ExecuteResponse{
	// 	Response: []byte(""),
	// }

	// return &response, nil
}

func (s *server) ApplyPolicy(
	ctx context.Context,
	in *ApplyPolicyRequest) (*ApplyPolicyResponse, error) {

	log.Print("ApplyPolicy was called.")
	log.Printf("Request: %s", in.Request)

	response := ApplyPolicyResponse{
		Success: true,
		Message: "",
	}
	return &response, nil
}

func main() {
	listener, err := net.Listen("tcp", ":8081")

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
