import type { UserDto } from "../types";

interface UserListProps {
    users: UserDto[];
}

const UserList = ({ users }: UserListProps) => {
    return (
        <div>
            <h2>Users</h2>

            {users.length === 0 && <p>No users found.</p>}

            <ul>
                {users.map(user => (
                    <li key={user.id}>
                        <strong>{user.firstName}  {user.lastName}</strong> 
                        <div className="text-sm text-gray-600">
                            {user.email} — {user.role}
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default UserList;
