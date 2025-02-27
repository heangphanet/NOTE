<template>
  <div class="max-w-7xl mx-auto p-6">
    <div class="flex items-center justify-between mb-6">
      <h1 class="text-3xl font-bold text-gray-800">My Notes</h1>
    </div>

    <div class="flex items-center mb-6 gap-4">
      <input v-model="searchQuery" type="text" placeholder="Search notes..." class="border rounded-lg px-4 py-2 w-1/2" />

      <select v-model="sortOption" class="border rounded-lg px-4 py-2">
        <option value="createdAtDesc">Sort by Newest</option>
        <option value="createdAtAsc">Sort by Oldest</option>
        <option value="titleAsc">Sort by Title (A-Z)</option>
        <option value="titleDesc">Sort by Title (Z-A)</option>
      </select>
    </div>

    <div class="flex gap-4 mb-6">
      <button @click="openCreateModal"
              class="bg-blue-600 text-white text-sm px-4 py-2 rounded-lg hover:bg-blue-700 transition-all"
              v-if="selectedNote === null && !isCreatingNewNote">
        Add Note
      </button>

      <button @click="openEditModal(selectedNote)"
              class="bg-yellow-600 text-white text-sm px-4 py-2 rounded-lg hover:bg-yellow-700 flex items-center gap-2 transition-all"
              v-if="selectedNote">
        Edit Note
      </button>

      <button @click="deleteNote(selectedNote.id, selectedNote.userId)"
              class="bg-red-600 text-white text-sm px-4 py-2 rounded-lg hover:bg-red-700 flex items-center gap-2 transition-all"
              v-if="selectedNote">
        Delete Note
      </button>

      <button @click="openDetailModal(selectedNote)"
              class="bg-yellow-600 text-white text-sm px-4 py-2 rounded-lg hover:bg-yellow-700 flex items-center gap-2 transition-all"
              v-if="selectedNote">
        View Note
      </button>
      <button @click="cancelSelection"
              class="bg-gray-500 text-white text-sm px-4 py-2 rounded-lg hover:bg-gray-600 flex items-center gap-2 transition-all"
              v-if="selectedNote">
        Cancel
      </button>
    </div>

    <div v-if="filteredAndSortedNotes.length > 0" class="overflow-x-auto">
      <table class="min-w-full bg-white border border-gray-300 rounded-lg shadow-lg">
        <thead>
          <tr class="bg-gray-100">
            <th class="text-sm font-semibold text-gray-800 px-6 py-3 text-left">Title</th>
            <th class="text-sm font-semibold text-gray-800 px-6 py-3 text-left">Content</th>
            <th class="text-sm font-semibold text-gray-800 px-6 py-3 text-left">Created At</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="note in filteredAndSortedNotes"
              :key="note.id"
              @click="selectNote(note)"
              :class="[ 'cursor-pointer transition-all duration-300', selectedNote?.id === note.id ? 'bg-blue-200' : 'hover:bg-gray-50']">
            <td class="text-sm text-gray-800 px-6 py-3">{{ note.title }}</td>
            <td class="text-sm text-gray-600 px-6 py-3 line-clamp-3">{{ note.content || 'No content' }}</td>
            <td class="text-sm text-gray-500 px-6 py-3">{{ formatDate(note.createdAt) }}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <div v-else class="text-center text-gray-500 py-12">No notes found. Click "Add Note" to create one.</div>

    <div v-if="isCreatingNewNote" class="bg-white p-4 rounded-xl shadow-lg mt-6">
      <div class="grid grid-cols-1 sm:grid-cols-2 gap-6">
        <div>
          <label for="title" class="block text-gray-600 mb-2">Title</label>
          <input v-model="newNote.title" type="text" id="title"
                 class="w-full border rounded px-3 py-2 text-gray-700" placeholder="Enter title" />
        </div>
        <div>
          <label for="content" class="block text-gray-600 mb-2">Content</label>
          <textarea v-model="newNote.content" id="content"
                    class="w-full border rounded px-3 py-2 text-gray-700" placeholder="Enter content" rows="4"></textarea>
        </div>
      </div>
      <div class="flex gap-2 mt-4 justify-end">
        <button @click="saveNewNote" class="bg-blue-600 text-white text-sm px-6 py-3 rounded-lg hover:bg-blue-700 transition-all">
          Save
        </button>
        <button @click="cancelCreateModal" class="bg-gray-600 text-white text-sm px-6 py-3 rounded-lg hover:bg-gray-700 transition-all">
          Cancel
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, computed, onMounted } from 'vue'
  import { NoteApi } from '@/api/notes'
  import { Note } from '@/types/note'
  import { useNoteStore } from '@/stores/notesStore';
  import { useRouter } from 'vue-router';

  const router = useRouter();

  const notes = ref<Note[]>([])
  const selectedNote = ref<Note | null>(null)
  const isCreatingNewNote = ref(false)
  const newNote = ref<Note>({
    id: 0,
    title: '',
    content: '',
    createdAt: new Date(),
    updatedAt: new Date(),
    userId: 1
  })

  const noteStore = useNoteStore();
  const userId = ref(1)

  const searchQuery = ref('') 
  const sortOption = ref('createdAtDesc') 

  const filteredAndSortedNotes = computed(() => {
    let filteredNotes = notes.value.filter(note =>
      note.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
      note.content?.toLowerCase().includes(searchQuery.value.toLowerCase())
    );

    if (sortOption.value === 'createdAtDesc') {
      filteredNotes.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
    } else if (sortOption.value === 'createdAtAsc') {
      filteredNotes.sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime());
    } else if (sortOption.value === 'titleAsc') {
      filteredNotes.sort((a, b) => a.title.localeCompare(b.title));
    } else if (sortOption.value === 'titleDesc') {
      filteredNotes.sort((a, b) => b.title.localeCompare(a.title));
    }

    return filteredNotes;
  })

  function formatDate(dateStr) {
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
  }

  async function fetchNotes() {
    try {
      const response = await NoteApi.getAll(userId.value)
      notes.value = response.data
    } catch (error) {
      console.error('Failed to fetch notes:', error)
    }
  }

  const openCreateModal = () => {
    isCreatingNewNote.value = true;
    newNote.value = { title: "", content: "" }; 
  };

  const openEditModal = (note: Note | null) => {
    if (note) {
      selectedNote.value = note;
      noteStore.getNote(note.userId, note.id);
      router.push(`/notes/edit/${note.userId}/${note.id}`);
    }
  };

  const openDetailModal = (note: Note | null) => {
    if (note) {
      selectedNote.value = note;
      noteStore.getNote(note.userId, note.id);   
      router.push(`/notes/Detail/${note.userId}/${note.id}`);
    }
  };

  function cancelSelection() {
    selectedNote.value = null;  
  }

  async function saveNewNote() {
    if (!newNote.value || !newNote.value.title?.trim()) {
      alert("Title is required.");
      return;
    }

    newNote.value.content = newNote.value.content ?? "";

    try {
      await noteStore.createNote(newNote.value);
      console.log("Note saved successfully!");
      isCreatingNewNote.value = false; 
    } catch (error) {
      console.error("Error saving note:", error);
      alert("Failed to save note. Check console for details.");
    }
  }

  function selectNote(note: Note) {
    selectedNote.value = note;
  }

  async function deleteNote(id: number, userId: number) {
    const confirmed = confirm("Are you sure you want to delete this note?");
    if (confirmed) {
      await noteStore.deleteNote(id, userId);
      notes.value = notes.value.filter(note => note.id !== id);
    }
  }

  function cancelCreateModal() {
    isCreatingNewNote.value = false; 
  }

  onMounted(fetchNotes);
</script>

<style scoped>
  .line-clamp-3 {
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }

  .bg-blue-200 {
    background-color: #bfdbfe !important;
  }

  .shadow-lg {
    box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
  }

  .grid {
    grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  }

  .fixed {
    position: fixed;
  }

  .bg-blue-600 {
    background-color: #2563eb;
  }

  .bg-gray-600 {
    background-color: #4b5563;
  }

  table {
    width: 100%;
    border-collapse: collapse;
  }

  th, td {
    padding: 1rem;
    text-align: left;
  }

  tr:hover {
    background-color: #f3f4f6;
  }

  td, th {
    border-bottom: 1px solid #e5e7eb;
  }

  button {
    margin-right: 0.5rem;
  }
</style>
