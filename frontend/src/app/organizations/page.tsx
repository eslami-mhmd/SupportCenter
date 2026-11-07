"use client";

import { useEffect, useState } from "react";
import { useLanguage } from "../../context/LanguageContext";
import { apiFetch } from "../../lib/api";
import { Building, Plus } from "lucide-react";

interface Organization {
  id: string;
  name: string;
  slug?: string;
}

export default function OrganizationsPage() {
  const { t } = useLanguage();
  const [orgs, setOrgs] = useState<Organization[]>([]);
  const [name, setName] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const fetchOrgs = () => {
    apiFetch<Organization[]>("/organizations")
      .then((data) => setOrgs(Array.isArray(data) ? data : []))
      .catch((err) => setError(err.message || "Failed to load organizations"))
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    fetchOrgs();
  }, []);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await apiFetch("/organizations", {
        method: "POST",
        body: JSON.stringify({ name }),
      });
      setName("");
      fetchOrgs();
    } catch (err: unknown) {
      alert((err as Error).message || "Failed to create organization");
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center space-x-3 rtl:space-x-reverse">
        <div className="p-2 bg-emerald-50 text-emerald-600 rounded-lg">
          <Building className="h-6 w-6" />
        </div>
        <h1 className="text-2xl font-bold text-gray-900">{t("organizations")}</h1>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <form onSubmit={handleCreate} className="bg-white shadow rounded-xl p-6 space-y-4 border border-gray-100 h-fit">
          <h2 className="text-lg font-bold text-gray-900">{t("create")} {t("organizations")}</h2>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">{t("subject")}</label>
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
              placeholder="Organization Name"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none"
            />
          </div>
          <button
            type="submit"
            className="w-full bg-emerald-600 text-white py-2.5 rounded-lg font-semibold hover:bg-emerald-700 transition flex items-center justify-center space-x-1 rtl:space-x-reverse"
          >
            <Plus className="h-5 w-5" />
            <span>{t("create")}</span>
          </button>
        </form>

        <div className="lg:col-span-2">
          {error && <div className="p-4 bg-red-50 text-red-700 rounded-lg mb-4">{error}</div>}
          {loading ? (
            <div className="text-center py-12 text-gray-500">{t("loading")}</div>
          ) : orgs.length === 0 ? (
            <div className="bg-white p-12 text-center rounded-xl shadow border border-gray-100 text-gray-500">
              No organizations found.
            </div>
          ) : (
            <div className="bg-white shadow rounded-xl overflow-hidden border border-gray-100">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                  <tr>
                    <th className="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase tracking-wider">ID</th>
                    <th className="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase tracking-wider">Name</th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {orgs.map((org) => (
                    <tr key={org.id} className="hover:bg-gray-50 transition">
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-mono text-gray-500">{org.id}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{org.name}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
