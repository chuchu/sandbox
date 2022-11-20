package main

import (
	"bufio"
	"encoding/json"
	"log"
	"net"
	"os"
	"strconv"

	grpc "google.golang.org/grpc"
)

type server struct {
	UnimplementedEventingServer
	name    string
	channel chan int
}

type payload struct {
	Message string
}

func (s *server) StartEventSource(
	in *EventSource,
	stream Eventing_StartEventSourceServer) error {

	log.Printf("StartEventSource was called from: '%s'", in.Name)

	for {
		number := <-s.channel

		for i := 0; i < number; i++ {

			p := payload{
				Message: "MyMessage",
			}

			pm, err := json.Marshal(p)

			if err != nil {
				log.Fatalf("Could not marshal payload: %v", err)
			}

			log.Printf("Send payload: '%v'", pm)

			event := Event{
				Name: s.name,
				//Payload: []byte(payload),
				Payload: pm,
			}
			stream.Send(&event)
		}
	}
}

func startGrpcServer(channel chan int) {
	listener, err := net.Listen("tcp", ":8080")

	if err != nil {
		log.Fatalf("failed to listen: %v", err)
	}

	grpcServer := grpc.NewServer()

	streamingServer := server{
		name:    "CustomEventSource",
		channel: channel,
	}

	RegisterEventingServer(grpcServer, &streamingServer)

	log.Printf("Server listening at %v", listener.Addr())

	if err := grpcServer.Serve(listener); err != nil {
		log.Fatalf("failed to serve: %v", err)
	}
}

func main() {
	channel := make(chan int)

	go startGrpcServer(channel)

	input := bufio.NewScanner(os.Stdin)

	for input.Scan() {
		i, err := strconv.Atoi(input.Text())
		if err != nil {
			log.Fatalf("Error: %v", err)
		}
		channel <- i
	}
}
