package main

import{
  "fmt"
  "queue"
}

func main(){
  q:=queue.New()
  
  if q.Empty() {
    q.Push(1)
  }
  defer fmt.Println("Now the queue is empty")
  defer q.Clean()
  fmt.Println("the top of the queue is",q.Top())
  q.Push(2)
  fmt.Println("the bottom of the queue is",q.Bottom())
  q.Pop()
  fmt.Println("the new top of the queue is",q.Top())
}
