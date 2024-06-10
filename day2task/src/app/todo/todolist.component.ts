// import { Component } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';


@Component({
  selector: 'app-todolist',
  templateUrl: './todolist.component.html',
  styleUrl: './todolist.component.css'
})
export class TodolistComponent implements OnInit {
  taskArray = [{ taskName: 'Take laundry', isCompleted: false }];

  constructor() { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm) {
    console.log(form);

    this.taskArray.push({
      taskName: form.controls['task'].value,
      isCompleted: false
    })

    form.reset();
  }

  onDelete(index: number) {
    console.log(index);

    this.taskArray.splice(index, 1);
  }

  onCheck(index: number) {
    console.log(this.taskArray);

    this.taskArray[index].isCompleted = !this.taskArray[index].isCompleted;
  }

  onDrop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.taskArray, event.previousIndex, event.currentIndex);
  }

  editIndex: number | null = null;

  onEdit(index: number): void {
    this.editIndex = index;
  }

  onEditEnd(): void {
    this.editIndex = null;
  }
}

