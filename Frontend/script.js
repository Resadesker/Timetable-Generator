// API endpoints
const API_BASE = 'https://localhost:7183/api/school';

// Initialize Select2 for multiple select
$(document).ready(function() {
    // Test API connection
    testApiConnection();
    
    $('.select2').select2({
        theme: 'bootstrap-5'
    });

    // Load initial data
    loadSubjects();
    loadTeachers();
    loadClasses();
    loadRooms();

    // Set up event listeners
    setupEventListeners();
});

async function testApiConnection() {
    try {
        const response = await fetch(`${API_BASE}/teachers`, {
            method: 'GET',
    headers: {
                'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
            mode: 'cors'
        });
        
        if (!response.ok) {
            throw new Error(`Server responded with status: ${response.status}`);
        }
        console.log('API connection successful');
    } catch (error) {
        console.error('API Connection Error:', error);
        showError(`Cannot connect to the server. Please ensure the backend is running at ${API_BASE}. Error: ${error.message}`);
    }
}

function setupEventListeners() {
    // Teacher form
    $('#teacherForm').on('submit', function(e) {
        e.preventDefault();
        const teacher = {
            name: $('#teacherName').val(),
            subjectIds: $('#teacherSubjects').val().map(id => parseInt(id)),
            availabilityJson: createDefaultAvailability()
        };
        createTeacher(teacher);
    });

    // Subject form
    $('#subjectForm').on('submit', function(e) {
        e.preventDefault();
        const subject = {
            name: $('#subjectName').val(),
            requiredRoomType: parseInt($('#roomType').val())
        };
        createSubject(subject);
    });

    // Class form
    $('#classForm').on('submit', function(e) {
  e.preventDefault();
        const className = $('#className').val();
        const requirements = [];
        
        $('.subject-requirement').each(function() {
            const subjectId = $(this).find('.subject-select').val();
            const hours = $(this).find('input[type="number"]').val();
            if (subjectId && hours) {
                requirements.push({
                    subjectId: parseInt(subjectId),
                    weeklyHours: parseInt(hours)
                });
            }
        });

        const schoolClass = {
            name: className,
            subjectRequirements: requirements
        };
        createClass(schoolClass);
    });

    // Room form
    $('#roomForm').on('submit', function(e) {
  e.preventDefault();
        const room = {
            name: $('#roomName').val(),
            roomType: parseInt($('#roomType').val()),
            availabilityJson: createDefaultAvailability()
        };
        createRoom(room);
    });

    // Add subject requirement
    $('#addRequirement').on('click', function() {
        const requirementHtml = `
            <div class="subject-requirement mb-2">
                <div class="row">
                    <div class="col">
                        <select class="form-control subject-select" required>
                            <option value="">Select Subject</option>
                            ${getSubjectOptions()}
                        </select>
                    </div>
                    <div class="col">
                        <input type="number" class="form-control" placeholder="Hours per week" min="1" max="40" required>
                    </div>
                    <div class="col-auto">
                        <button type="button" class="btn btn-danger remove-requirement">×</button>
                    </div>
                </div>
            </div>
        `;
        $('#subjectRequirements').append(requirementHtml);
    });

    // Remove subject requirement
    $(document).on('click', '.remove-requirement', function() {
        $(this).closest('.subject-requirement').remove();
    });

    // Generate timetable
    $('#generateTimetable').on('click', function() {
        generateTimetable();
    });
}

// API calls
async function loadSubjects() {
    try {
        const response = await fetch(`${API_BASE}/subjects`);
        const subjects = await response.json();
        updateSubjectList(subjects);
        updateSubjectSelects(subjects);
    } catch (error) {
        showError('Error loading subjects');
    }
}

async function loadTeachers() {
    try {
        const response = await fetch(`${API_BASE}/teachers`, {
            method: 'GET',
    headers: {
                'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
            mode: 'cors'
        });
        if (!response.ok) {
            const errorText = await response.text();
            console.error('Error loading teachers:', response.status, errorText);
            throw new Error(`Failed to load teachers: ${response.status} ${errorText}`);
        }
        const teachers = await response.json();
        updateTeacherList(teachers);
    } catch (error) {
        console.error('Error in loadTeachers:', error);
        showError(`Error loading teachers: ${error.message}`);
    }
}

async function loadClasses() {
    try {
        const response = await fetch(`${API_BASE}/classes`);
        if (!response.ok) {
            const errorText = await response.text();
            console.error('Error loading classes:', response.status, errorText);
            throw new Error(`Failed to load classes: ${response.status} ${errorText}`);
        }
        const classes = await response.json();
        updateClassList(classes);
    } catch (error) {
        console.error('Error in loadClasses:', error);
        showError(`Error loading classes: ${error.message}`);
    }
}

async function loadRooms() {
    try {
        const response = await fetch(`${API_BASE}/rooms`);
        const rooms = await response.json();
        updateRoomList(rooms);
    } catch (error) {
        showError('Error loading rooms');
    }
}

async function createTeacher(teacher) {
    try {
        const response = await fetch(`${API_BASE}/teachers`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(teacher)
        });
        if (!response.ok) {
            const errorText = await response.text();
            console.error('Error creating teacher:', response.status, errorText);
            throw new Error(`Failed to create teacher: ${response.status} ${errorText}`);
        }
        $('#teacherForm')[0].reset();
        loadTeachers();
    } catch (error) {
        console.error('Error in createTeacher:', error);
        showError(`Error creating teacher: ${error.message}`);
    }
}

async function createSubject(subject) {
    try {
        const response = await fetch(`${API_BASE}/subjects`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(subject)
        });
        if (response.ok) {
            $('#subjectForm')[0].reset();
            loadSubjects();
        } else {
            showError('Error creating subject');
        }
    } catch (error) {
        showError('Error creating subject');
    }
}

async function createClass(schoolClass) {
    try {
        const response = await fetch(`${API_BASE}/classes`, {
    method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(schoolClass)
        });
        if (!response.ok) {
            const errorText = await response.text();
            console.error('Error creating class:', response.status, errorText);
            throw new Error(`Failed to create class: ${response.status} ${errorText}`);
        }
        $('#classForm')[0].reset();
        $('#subjectRequirements').html(getInitialRequirementHtml());
        loadClasses();
    } catch (error) {
        console.error('Error in createClass:', error);
        showError(`Error creating class: ${error.message}`);
    }
}

async function createRoom(room) {
    try {
        const response = await fetch(`${API_BASE}/rooms`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(room)
        });
        if (response.ok) {
            $('#roomForm')[0].reset();
            loadRooms();
        } else {
            showError('Error creating room');
        }
    } catch (error) {
        showError('Error creating room');
    }
}

async function generateTimetable() {
    try {
        $('#generateTimetable').prop('disabled', true).addClass('loading');
        const response = await fetch(`${API_BASE}/generate-timetable`, {
    method: 'POST'
  });
        if (response.ok) {
            const timetable = await response.json();
            displayTimetable(timetable);
        } else {
            const error = await response.text();
            showError(`Error generating timetable: ${error}`);
        }
    } catch (error) {
        showError('Error generating timetable');
    } finally {
        $('#generateTimetable').prop('disabled', false).removeClass('loading');
    }
}

// Helper functions
function createDefaultAvailability() {
    const availability = {};
    ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'].forEach(day => {
        availability[day] = Array(10).fill(true);
    });
    return JSON.stringify(availability);
}

function getSubjectOptions() {
    return $('#teacherSubjects option').clone();
}

function getInitialRequirementHtml() {
    return `
        <div class="subject-requirement mb-2">
            <div class="row">
                <div class="col">
                    <select class="form-control subject-select" required>
                        <option value="">Select Subject</option>
                        ${getSubjectOptions()}
                    </select>
                </div>
                <div class="col">
                    <input type="number" class="form-control" placeholder="Hours per week" min="1" max="40" value="2" required>
                </div>
                <div class="col-auto">
                    <button type="button" class="btn btn-danger remove-requirement">×</button>
                </div>
            </div>
        </div>
    `;
}

// UI update functions
function updateSubjectList(subjects) {
    const html = subjects.map(subject => `
        <div class="list-group-item">
            <h6 class="mb-1">${subject.name}</h6>
            <small class="text-muted">Room Type: ${subject.requiredRoomType}</small>
        </div>
    `).join('');
    $('#subjectList').html(html);
}

function updateTeacherList(teachers) {
    const html = teachers.map(teacher => `
        <div class="list-group-item">
            <h6 class="mb-1">${teacher.name}</h6>
            <small class="text-muted">Subjects: ${teacher.canTeachSubjects.map(s => s.name).join(', ')}</small>
        </div>
    `).join('');
    $('#teacherList').html(html);
}

function updateClassList(classes) {
    const html = classes.map(schoolClass => `
        <div class="list-group-item">
            <h6 class="mb-1">${schoolClass.name}</h6>
            <small class="text-muted">
                Requirements: ${schoolClass.subjectRequirements.map(r => 
                    `${r.subject.name} (${r.weeklyHours}h)`
                ).join(', ')}
            </small>
        </div>
    `).join('');
    $('#classList').html(html);
}

function updateRoomList(rooms) {
    const html = rooms.map(room => `
        <div class="list-group-item">
            <h6 class="mb-1">${room.name}</h6>
            <small class="text-muted">Type: ${room.roomType}</small>
        </div>
    `).join('');
    $('#roomList').html(html);
}

function updateSubjectSelects(subjects) {
    const options = subjects.map(subject => 
        `<option value="${subject.id}">${subject.name}</option>`
    ).join('');
    $('.subject-select').html(`<option value="">Select Subject</option>${options}`);
    $('#teacherSubjects').html(options);
}

function displayTimetable(timetable) {
    const days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'];
    const periods = Array.from({length: 10}, (_, i) => i + 1);

    let html = '<div class="table-responsive"><table class="table table-bordered">';
    
    // Header row with periods
    html += '<thead><tr><th>Class</th>';
    periods.forEach(period => {
        html += `<th>Period ${period}</th>`;
    });
    html += '</tr></thead>';

    // Group lessons by class
    const lessonsByClass = timetable.reduce((acc, lesson) => {
        if (!acc[lesson.class.name]) {
            acc[lesson.class.name] = {};
        }
        if (!acc[lesson.class.name][lesson.timeSlot.day]) {
            acc[lesson.class.name][lesson.timeSlot.day] = {};
        }
        acc[lesson.class.name][lesson.timeSlot.day][lesson.timeSlot.period] = lesson;
        return acc;
    }, {});

    // Create rows for each class
    Object.keys(lessonsByClass).sort().forEach(className => {
        html += `<tr><th>${className}</th>`;
        periods.forEach(period => {
            const dayLessons = days.map(day => {
                const lesson = lessonsByClass[className][day]?.[period];
                if (lesson) {
                    return `
                        <div class="lesson">
                            <strong>${lesson.subject.name}</strong><br>
                            ${lesson.teacher.name}<br>
                            ${lesson.room.name}
                        </div>
                    `;
                }
                return '<div class="lesson empty"></div>';
            });
            html += `<td>${dayLessons.join('')}</td>`;
        });
        html += '</tr>';
    });

    html += '</table></div>';
    $('#timetableResult').html(html);
}

function showError(message) {
    console.error('Error:', message);
    alert(message);
}
