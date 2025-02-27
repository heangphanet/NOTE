<template>
  <div v-if="note" class="max-w-4xl mx-auto p-6 bg-white rounded-lg shadow-lg">
    <h1 class="text-3xl font-semibold text-gray-800 mb-6">Note Details</h1>

    <div class="space-y-4">
      <!-- Title Section -->
      <div class="flex">
        <label class="text-gray-700 font-semibold mr-4">Title:</label>
        <p class="text-lg text-gray-600">{{ note.title }}</p>
      </div>

      <!-- Content Section -->
      <div class="flex">
        <label class="text-gray-700 font-semibold mr-4">Content:</label>
        <p class="text-lg text-gray-600">{{ note.content || "No content available." }}</p>
      </div>

      <!-- Created At Section -->
      <div class="flex">
        <label class="text-gray-700 font-semibold mr-4">Created At:</label>
        <p class="text-gray-600"> {{ formatDate(note.createdAt) }}</p>
      </div>

      <!-- Last Updated Section -->
      <div class="flex">
        <label class="text-gray-700 font-semibold mr-4">Last Updated:</label>
        <p class="text-gray-600">{{formatDate(note.updatedAt) }}</p>
      </div>
    </div>

    <!-- Back Button -->
    <div class="mt-6 text-center">
      <button @click="goBack" class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded-md shadow-md transition-all hover:shadow-lg">
        Back to Notes
      </button>
    </div>
  </div>

  <!-- Loading state -->
  <div v-else class="text-center text-gray-600 py-6">Loading...</div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from "vue";
  import { useRoute, useRouter } from "vue-router";
  import { useNoteStore } from "@/stores/notesStore";
  import type { Note } from "../types/note";

  const route = useRoute();
  const router = useRouter();
  const note = ref<Note | null>(null);
  const noteStore = useNoteStore();
  const formatDate = (dateStr: string | null) => {
  if (!dateStr) return "Not updated yet"; 
  const date = new Date(dateStr);
  return date.toLocaleString('en-US', {
    month: 'short',
    day: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    hour12: true
  });
};
  const fetchNoteDetail = async (id: number) => {
    try {
      const userId = 1;
      await noteStore.getNote(userId, id); 
      note.value = noteStore.selectedNote; 
    } catch (error) {
      console.error("Failed to fetch note details:", error);
    }
  };

  onMounted(() => {
    const noteId = Number(route.params.id); 
    fetchNoteDetail(noteId);
  });

  const goBack = () => {
    router.push("/"); 
  };
</script>

<style scoped>
  .text-lg {
    font-size: 1.125rem;
  }

  .space-y-4 {
    margin-bottom: 1rem;
  }

  .flex {
    display: flex;
    align-items: center;
  }

  .mr-4 {
    margin-right: 1rem;
  }
</style>
