import { defineStore } from 'pinia';
import type { Note } from '@/types/note';
import { NoteApi } from '@/api/notes';
import axios, { AxiosError } from "axios";
export const useNoteStore = defineStore('notes', {
  state: () => ({
    notes: [] as Note[],
    searchQuery: '',
    selectedNote: null as Note | null, 
  }),
  actions: {
    async fetchNotes() {
      try {
        const response = await NoteApi.getAll(1); 
        this.notes = response.data;
      } catch (error) {
        console.error('Failed to fetch notes:', error);
      }
    },
    async updateNote(id: number, userId: number, note: Note) {
      note.createdAt = new Date().toISOString();
      note.updatedAt = new Date().toISOString();
      try {
        await NoteApi.update(id, userId, note); 
        alert("Note saved successfully!");
        await this.fetchNotes(); 
      } catch (error) {
        console.error('Failed to update note:', error);
      }
    },
    async createNote(note: Note) {
      const userId = 1; 

      if (!note.title?.trim()) {
        console.error("Title is required");
        alert("Title cannot be empty.");
        return;
      }

      note.createdAt = new Date().toISOString();
      note.updatedAt = ""; 
      note.content = note.content ?? ""; 

      try {
        const response = await NoteApi.create(userId, note);
        await this.fetchNotes(); 
      } catch (error: unknown) {
        if (error instanceof AxiosError) {
          console.error("Failed to create note:", error.response?.data || error.message);
          alert(`Failed to create note: ${error.response?.data?.message || error.message}`);
        } else {
          console.error("An unexpected error occurred:", error);
          alert("An unexpected error occurred.");
        }
      }
    },


    async getNote(userId: number, id: number) {
      try {
        const response = await NoteApi.get(userId, id); 
        this.selectedNote = response.data; 
      } catch (error) {
        console.error('Failed to fetch note for editing:', error);
      }
    },
    async deleteNote(id: number) {
      const userId = 1; 
      try {
        await NoteApi.delete(id.toString(), userId); 
        await this.fetchNotes(); 
      } catch (error) {
        console.error('Failed to delete note:', error);
      }
    }
  },
  getters: {
    filteredNotes(state) {
      return state.notes.filter(note =>
        note.title.toLowerCase().includes(state.searchQuery.toLowerCase()));
    }
  }
});
