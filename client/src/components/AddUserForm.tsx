import React, { useState } from "react";
import type { CreateUserDto } from "../types";

interface AddUserFormProps {
    onSubmit: (user: CreateUserDto) => void;
    onCancel: () => void;
}

const AddUserForm: React.FC<AddUserFormProps> = ({ onSubmit, onCancel }) => {
    const [user, setUser] = useState<CreateUserDto>({
        firstName: "",
        lastName: "",
        email: "",
        role: "", 
    });

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
    ) => {
        setUser({ ...user, [e.target.name]: e.target.value });
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit(user);
    };

    return (
        <div className="p-6"> 
            <h2 className="text-xl font-bold mb-4 text-gray-800">Add User</h2>

            <form onSubmit={handleSubmit} className="space-y-4">
                <div className="grid grid-cols-2 gap-4">
                    <div>
                        <label className="text-xs font-bold text-gray-500 uppercase">First Name</label>
                        <input
                            name="firstName"
                            value={user.firstName}
                            onChange={handleChange}
                            className="w-full border p-2 rounded-lg mt-1 focus:ring-2 focus:ring-blue-500 outline-none"
                            required
                        />
                    </div>
                    <div>
                        <label className="text-xs font-bold text-gray-500 uppercase">Last Name</label>
                        <input
                            name="lastName"
                            value={user.lastName}
                            onChange={handleChange}
                            className="w-full border p-2 rounded-lg mt-1 focus:ring-2 focus:ring-blue-500 outline-none"
                            required
                        />
                    </div>
                </div>

                <div>
                    <label className="text-xs font-bold text-gray-500 uppercase">Email</label>
                    <input
                        type="email"
                        name="email"
                        value={user.email}
                        onChange={handleChange}
                        className="w-full border p-2 rounded-lg mt-1 focus:ring-2 focus:ring-blue-500 outline-none"
                        required
                    />
                </div>

                <div>
                    <label className="text-xs font-bold text-gray-500 uppercase">Role</label>
                    <select
                        name="role"
                        value={user.role}
                        onChange={handleChange}
                        className="w-full border p-2 rounded-lg mt-1 focus:ring-2 focus:ring-blue-500 outline-none bg-white"
                        required
                    >
                        <option value="" disabled>Select a Role</option>
                        <option value="Partner">Partner</option>
                        <option value="Employee">Employee</option>
                    </select>
                </div>

                <div className="flex gap-3 mt-6">
                    <button
                        type="submit"
                        className="flex-1 bg-blue-600 text-white py-2 rounded-lg font-semibold hover:bg-blue-700 transition"
                    >
                        Save User
                    </button>
                    <button
                        type="button"
                        onClick={onCancel}
                        className="flex-1 bg-gray-100 text-gray-700 py-2 rounded-lg font-semibold hover:bg-gray-200 transition"
                    >
                        Cancel
                    </button>
                </div>
            </form>
        </div>
    );
};

export default AddUserForm;
