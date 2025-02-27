<template>
  <div v-if="note">
    <h1>Edit Note: {{ note.title }}</h1>

    <!-- Note Editing Form -->
    <form>
      <label for="title">Title</label>
      <input v-model="note.title" type="text" id="title" />

      <label for="content">Content</label>
      <textarea v-model="note.content" id="content"></textarea>

      <button type="submit" @click.prevent="saveNote">Save</button>
    </form>
  </div>
  <div v-else>
    <p>Loading...</p>
  </div>
</template>

<script setup lang="ts">
  import { useRoute, useRouter } from "vue-router";
  import { onMounted, ref } from 'vue';  
  import { useNoteStore } from '@/stores/notesStore'; 
  import { Note } from '@/types/note';
  const userId = 1;
  const noteStore = useNoteStore(); 
  const router = useRouter();
  const route = useRoute(); 

  const note = ref<Note | null>(null); 

  onMounted(async () => {
    const noteId = route.params.id as string;

    await noteStore.getNote(userId, parseInt(noteId)); 
    note.value = noteStore.selectedNote;
  });

  const saveNote = async () => {
    if (note.value) {
      try {
        await noteStore.updateNote(note.value.id, userId, note.value); 
        router.push("/"); 
      } catch (error) {
        console.error("Failed to save note:", error);
        alert("Failed to save note.");
      }
    }
  };
</script>
