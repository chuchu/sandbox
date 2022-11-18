package main

import (
	"log"

	"github.com/nats-io/nats.go"
)

func main() {
	nc, err := nats.Connect(nats.DefaultURL)
	handleError("Connect to NATS system.", err)

	js, err := nc.JetStream(nats.PublishAsyncMaxPending(256))
	handleError("Create JetStream context.", err)

	_, err = js.Publish("ORDERS.scratch", []byte("hello"))
	handleError("Create JetStream context.", err)

	_, err = js.AddStream(&nats.StreamConfig{
		Name:     "orders",
		Subjects: []string{"ORDERS.*"},
	})
	handleError("Add stream.", err)
}

func handleError(action string, err error) {
	if err != nil {
		log.Fatalf("%s - %v", action, err)
	}
}
