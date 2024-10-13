document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    // Function to format date as 'YYYY-MM-DDTHH:mm' for datetime-local input
    function formatLocalDate(date) {
        var year = date.getFullYear();
        var month = String(date.getMonth() + 1).padStart(2, '0');
        var day = String(date.getDate()).padStart(2, '0');
        var hours = String(date.getHours()).padStart(2, '0');
        var minutes = String(date.getMinutes()).padStart(2, '0');
        return `${year}-${month}-${day}T${hours}:${minutes}`;
    }

    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        selectable: true,
        select: function (info) {
            // Open modal to create a new schedule
            document.getElementById('scheduleId').value = '';
            document.getElementById('title').value = '';
            document.getElementById('description').value = '';

            // Format the start and end time for datetime-local input
            document.getElementById('startTime').value = formatLocalDate(new Date(info.start));
            document.getElementById('endTime').value = formatLocalDate(new Date(info.end));

            document.getElementById('coachId').value = '';  // Reset the coach dropdown
            document.getElementById('scheduleModal').style.display = 'block';
        },
        eventClick: function (info) {
            // Open modal to edit an existing schedule
            var event = info.event;

            document.getElementById('scheduleId').value = event.id;
            document.getElementById('title').value = event.title;
            document.getElementById('description').value = event.extendedProps.description;

            // Format the start and end time for datetime-local input
            document.getElementById('startTime').value = formatLocalDate(new Date(event.start));

            if (event.end) {
                document.getElementById('endTime').value = formatLocalDate(new Date(event.end));
            } else {
                document.getElementById('endTime').value = '';
            }

            document.getElementById('coachId').value = event.extendedProps.coachId;  // Set the coach selection
            document.getElementById('scheduleModal').style.display = 'block';
        },
        events: '/Schedules/GetSchedules'
    });

    calendar.render();
});
