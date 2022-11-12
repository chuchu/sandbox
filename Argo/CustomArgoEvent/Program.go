package main

import (
	"log"
	"net"
	"time"

	grpc "google.golang.org/grpc"
)

type server struct {
	UnimplementedEventingServer
}

func (s *server) StartEventSource(in *EventSource, stream Eventing_StartEventSourceServer) error {
	for {
		event := Event{
			Name:    "NewEvent",
			Payload: []byte("1.2.3"),
		}
		stream.Send(&event)
		time.Sleep(5 * time.Second)
	}
}

func main() {
	lis, err := net.Listen("tcp", "127.0.0.1:8080")
	if err != nil {
		log.Fatalf("failed to listen: %v", err)
	}
	_ = lis

	s := grpc.NewServer()

	RegisterEventingServer(s, &server{})

	log.Printf("server listening at %v", lis.Addr())

	if err := s.Serve(lis); err != nil {
		log.Fatalf("failed to serve: %v", err)
	}
}
