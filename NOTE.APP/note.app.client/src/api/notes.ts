import axios from "axios";
import type { Note } from "../types/note";

const BASE_URL = "https://localhost:7254/api/notes";

export const NoteApi = {
  // Fetch all notes for a user
  getAll: (userId: number) => axios.get(`${BASE_URL}/${userId}`),

  // Create a new note for a user
  create: (Userid:number,note: Note) =>
    axios.post(`${BASE_URL}/${Userid}`, note)
, 

  get: async (userId: number,noteId: number) => {
    return axios.get(`${BASE_URL}/${userId}/${noteId}`);
  },

  async update(noteId: number, userId: number, note: Note) {
    return axios.put(`${BASE_URL}/${userId}/${noteId}`, note); 
  },
  getOne: async (noteId: string, userId: string) => {
    try {
      const response = await axios.get(`${BASE_URL}/notes/${noteId}?userId=${userId}`);
      return response.data;
    } catch (error) {
      throw new Error('Error fetching note');
    }
  },
  // Delete a note for a user
  async delete(noteId: string, userId: number) {
    return axios.delete(`${BASE_URL}/${userId}/${noteId}`); 
  }
};
