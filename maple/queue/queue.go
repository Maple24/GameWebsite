package queue

type node struct {
	Value float32
	next  *node
}

type queue struct {
	front *node
	rear  *node
}

func New() *queue {
	var q *queue = new(queue)
	return q
}

func (this *queue) Empty() bool {
	if this.front == this.rear && this.front == nil {
		return true
	}
	return false
}

func (this *queue) Push(value float32) {
	new_node := &node{value, nil}
	if this.Empty() {
		this.front = new_node
		this.rear = new_node
		return
	}
	p := this.front
	for p.next != nil {
		p = p.next
	}
	p.next = new_node
	this.rear = new_node
	return
}

func (this *queue) Pop() bool {
	if this.Empty() {
		return false
	} else {
		if this.front == this.rear {
			this.front = nil
			this.rear = nil
			return true
		}
		p := this.front
		this.front = p.next
		return true
	}
}

func (this *queue) Top() float32 {
	if this.Empty() {
		return -1
	}
	return this.front.Value
}

func (this *queue) Bottom() float32 {
	if this.Empty() {
		return -1
	}
	return this.rear.Value
}

func (this *queue) Clean() bool {
	if this.Empty() {
		return false
	}
	this.front = nil
	this.rear = nil
	return true
}
