<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50 transition-all">
    <div class="bg-white rounded-xl shadow-xl w-full max-w-2xl max-h-[90vh] overflow-auto">
      <!-- Modal Header -->
      <div class="flex items-center justify-between p-4 border-b">
        <h2 class="text-xl font-semibold text-gray-800">{{ isNewNote ? 'New Note' : 'Edit Note' }}</h2>
        <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700">
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      <!-- Form Section -->
      <form @submit.prevent="handleSubmit" class="p-6 space-y-6">
        <!-- Title Field -->
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Title</label>
          <input v-model="form.title"
                 required
                 placeholder="Enter note title"
                 class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-colors" />
        </div>

        <!-- Content Field -->
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Content</label>
          <textarea v-model="form.content"
                    rows="8"
                    placeholder="Write your note here..."
                    class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-colors"></textarea>
        </div>

        <!-- Buttons -->
        <div class="flex justify-end gap-4">
          <button type="button"
                  @click="$emit('close')"
                  class="px-4 py-2 text-gray-700 hover:bg-gray-50 rounded-lg transition-colors focus:outline-none">
            Cancel
          </button>
          <button type="submit"
                  class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors focus:outline-none">
            Save Note
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, computed, watch } from 'vue';
  import { NoteApi } from '@/api/notes';  

  const props = defineProps({
    note: { type: Object, required: true }
  });

  const emit = defineEmits(['close', 'saved']);

  const form = ref({
    title: props.note.title || '',
    content: props.note.content || ''
  });

  const isNewNote = computed(() => !props.note.id);

  async function handleSubmit() {
    try {
      const url = isNewNote.value
        ? '/api/notes'
        : `/api/notes/${props.note.id}`;

      const method = isNewNote.value ? 'post' : 'put';

      await NoteApi[method](url, {
        ...form.value,
        userId: props.note.userId,
      });

      emit('saved');
      emit('close'); 
    } catch (error) {
      console.error('Failed to save note:', error);
    }
  }

  watch(() => props.note, (newVal) => {
    form.value = {
      title: newVal.title || '',
      content: newVal.content || ''
    };
  }, { immediate: true });
</script>

<style scoped>
  input:focus, textarea:focus {
    box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.5);
  }

  .transition-all {
    transition: all 0.3s ease;
  }

  .bg-opacity-50 {
    background-color: rgba(0, 0, 0, 0.5);
  }
</style>
