import React, { Component } from 'react';
import axios from 'axios';

class TaskList extends Component {
    state = {
        date: new Date(), // початкова дата
        tasks: [],
        newTask: {
            title: '',
            description: '',
            dueDate: '',
            completed: false
        }
    };

    componentDidMount() {
        this.getTasks();
    }

    getTasks = () => {
        const { date } = this.state;
        axios.get(`/api/tasks?date=${date.toISOString()}`)
            .then(response => {
                this.setState({ tasks: response.data });
            })
            .catch(error => {
                console.error('Error fetching tasks:', error);
            });
    }

    handleDateChange = event => {
        this.setState({ date: new Date(event.target.value) }, () => {
            this.getTasks();
        });
    }

    // Додавання нової справи
    addTask = () => {
        const { newTask } = this.state;
        axios.post('/api/tasks', newTask)
            .then(response => {
                this.setState({ newTask: { title: '', description: '', dueDate: '', completed: false } });
                this.getTasks();
            })
            .catch(error => {
                console.error('Error adding task:', error);
            });
    }

    // Редагування справи
    editTask = task => {
        axios.put(`/api/tasks/${task.id}`, task)
            .then(() => {
                this.getTasks();
            })
            .catch(error => {
                console.error('Error editing task:', error);
            });
    }

    // Видалення справи
    deleteTask = taskId => {
        axios.delete(`/api/tasks/${taskId}`)
            .then(() => {
                this.getTasks();
            })
            .catch(error => {
                console.error('Error deleting task:', error);
            });
    }

    // Відмітити справу як виконану або навпаки
    toggleTaskCompleted = (taskId, completed) => {
        axios.patch(`/api/tasks/${taskId}/completed`, completed)
            .then(() => {
                this.getTasks();
            })
            .catch(error => {
                console.error('Error toggling task completion:', error);
            });
    }

    render() {
        const { tasks, newTask, date } = this.state;

        return (
            <div>
                <h1>Список справ на обрану дату</h1>
                <input
                    type="date"
                    value={date.toISOString().split('T')[0]}
                    onChange={this.handleDateChange}
                />
                <ul>
                    {tasks.map(task => (
                        <li key={task.id}>
                            {task.title}
                            <button onClick={() => this.editTask(task)}>Редагувати</button>
                            <button onClick={() => this.deleteTask(task.id)}>Видалити</button>
                            <input
                                type="checkbox"
                                checked={task.completed}
                                onChange={() => this.toggleTaskCompleted(task.id, !task.completed)}
                            />
                        </li>
                    ))}
                </ul>
                <div>
                    <h2>Додати нову справу</h2>
                    <input
                        type="text"
                        placeholder="Назва"
                        value={newTask.title}
                        onChange={e => this.setState({ newTask: { ...newTask, title: e.target.value } })}
                    />
                    <input
                        type="text"
                        placeholder="Опис"
                        value={newTask.description}
                        onChange={e => this.setState({ newTask: { ...newTask, description: e.target.value } })}
                    />
                    <input
                        type="date"
                        value={newTask.dueDate}
                        onChange={e => this.setState({ newTask: { ...newTask, dueDate: e.target.value } })}
                    />
                    <button onClick={this.addTask}>Додати</button>
                </div>
            </div>
        );
    }
}

export default TaskList;