using System;
using System.Collections.Generic;

class Todo {
  public string text;
  public bool completed;
}

class TodosStore {
  public List<Todo> todos = new List<Todo>();
  public ConsoleKey completeKey = ConsoleKey.C;
  public ConsoleKey deleteKey = ConsoleKey.D;
  public ConsoleKey editKey = ConsoleKey.E;

  public void Init() {
    this.AddTodo();
  }

  public void AddTodo() {
    Console.Write("Enter todo: ");
    string todoText = Console.ReadLine();

    if (todoText == Info.infoCommand) {
      Info.Instructions(true);
      this.AddTodo();
    }

    int todoIndex;
    if (int.TryParse(todoText, out todoIndex)) {
      try {
        this.SelectTodo(todoIndex);
        return;
      }
      catch {}
    }

    if (!string.IsNullOrEmpty(todoText)) {
      this.todos.Add(new Todo () {
        text = todoText,
        completed = false,
      });
    }

    this.Redraw();
  }

  public void DrawTodos() {
    this.todos.ForEach(delegate (Todo todo) {
      Console.WriteLine($"{this.todos.IndexOf(todo) + 1}. {todo.text}" + (todo.completed ? " (completed)" : ""));
    });
  }

  public void Redraw() {
    Console.Clear();
    this.DrawTodos();
    Console.WriteLine("");
    this.AddTodo();
  }

  public void SelectTodo(int index) {
    Todo selectedTodo = this.todos[index - 1];
    Console.WriteLine($"\nYou selected todo number {index}.\nSelected todo: {selectedTodo.text}");
    Console.Write($"\nPress: \n\"{this.completeKey}\" to complete/uncomplete\n\"{this.deleteKey}\" to delete\n\"{this.editKey}\" to edit\nAny key to close");
    
    switch (Console.ReadKey().Key) {
      case ConsoleKey.C:
        this.CompleteTodo(index - 1);
        break;
      case ConsoleKey.D:
        this.DeleteTodo(index - 1);
        break;
      case ConsoleKey.E:
        this.EditTodo(index - 1);
        break;
      default:
        this.Redraw();
        break;
    }
  }

  public void CompleteTodo(int index) {
    this.todos[index].completed = !this.todos[index].completed;
    this.Redraw();
  }

  public void DeleteTodo(int index) {
    this.todos.Remove(this.todos[index]);
    this.Redraw();
  }

  public void EditTodo(int index) {
    Console.Write("\n\nEnter a new text of the todo: ");
    string text = Console.ReadLine();

    if (!string.IsNullOrEmpty(text)) {
      this.todos[index].text = text;
    }

    this.Redraw();
  }
}

class Info {
  public static string infoCommand = "info";
  public static void Init() {
    Console.WriteLine($"Hi, in the TodoApp! Write you tasks below and be happy after completing :)\nType \"{Info.infoCommand}\" to read the instructions.");
    Info.Instructions();
  }

  public static void Instructions(bool newLine = false) {
    Console.WriteLine((newLine ? "\n" : "") + "Type number of a todo to select it.\n");
  }
}

namespace TodoList {
  class App {
    public static void Main(string[] args) {
      Info.Init();

      TodosStore todosStore = new TodosStore();
      todosStore.Init();
    }
  }
}
