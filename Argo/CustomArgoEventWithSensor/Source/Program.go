package main

import (
	"bufio"
	"fmt"
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

func (s *server) StartEventSource(
	in *EventSource,
	stream Eventing_StartEventSourceServer) error {

	log.Printf("StartEventSource was called from: '%s'", in.Name)

	for {
		number := <-s.channel
		for i := 0; i < number; i++ {
			log.Printf("Send event: %d", i)
			event := Event{
				Name:    s.name,
				Payload: []byte(fmt.Sprintf("Event %d", i)),
			}
			stream.Send(&event)
		}
	}
}

func startGrpcServer(channel chan int) {
	listener, err := net.Listen("tcp", "127.0.0.1:8080")

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
		fmt.Print("Please enter the number of events to be raised: ")
		i, err := strconv.Atoi(input.Text())
		if err != nil {
			log.Fatalf("Error: %v", err)
		}
		channel <- i
	}
}
