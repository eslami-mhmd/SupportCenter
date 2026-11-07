"use client";

import { useEffect, useState } from "react";
import { useLanguage } from "@/context/LanguageContext";
import { apiFetch } from "@/lib/api";
import { ShieldCheck, Plus } from "lucide-react";

interface SlaPolicy {
  id: string;
  name: string;
  priority: string;
  responseTimeMinutes: number;
  resolutionTimeMinutes: number;
}

export default function SlaPage() {
  const { t } = useLanguage();
  const [policies, setPolicies] = useState<SlaPolicy[]>([]);
  const [name, setName] = useState("");
  const [priority, setPriority] = useState("Medium");
  const [responseTimeMinutes, setResponseTimeMinutes] = useState(60);
  const [resolutionTimeMinutes, setResolutionTimeMinutes] = useState(480);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const fetchPolicies = () => {
    apiFetch<SlaPolicy[]>("/sla-policies")
      .then((data) => setPolicies(Array.isArray(data) ? data : []))
      .catch((err) => setError(err.message || "Failed to load SLA policies"))
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    fetchPolicies();
  }, []);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await apiFetch("/sla-policies", {
        method: "POST",
        body: JSON.stringify({ name, priority, responseTimeMinutes, resolutionTimeMinutes }),
      });
      setName("");
      fetchPolicies();
    } catch (err: unknown) {
      alert((err as Error).message || "Failed to create SLA policy");
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center space-x-3 rtl:space-x-reverse">
        <div className="p-2 bg-amber-50 text-amber-600 rounded-lg">
          <ShieldCheck className="h-6 w-6" />
        </div>
        <h1 className="text-2xl font-bold text-gray-900">{t("slaPolicies")}</h1>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <form onSubmit={handleCreate} className="bg-white shadow rounded-xl p-6 space-y-4 border border-gray-100 h-fit">
          <h2 className="text-lg font-bold text-gray-900">{t("create")} SLA Policy</h2>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">{t("subject")}</label>
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
              placeholder="Policy Name"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">{t("priority")}</label>
            <select
              value={priority}
              onChange={(e) => setPriority(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg bg-white"
            >
              <option value="Low">{t("low")}</option>
              <option value="Medium">{t("medium")}</option>
              <option value="High">{t("high")}</option>
              <option value="Urgent">{t("urgent")}</option>
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Response Time (mins)</label>
            <input
              type="number"
              value={responseTimeMinutes}
              onChange={(e) => setResponseTimeMinutes(Number(e.target.value))}
              required
              className="w-full px-3 py-2 border border-gray-300 rounded-lg"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Resolution Time (mins)</label>
            <input
              type="number"
              value={resolutionTimeMinutes}
              onChange={(e) => setResolutionTimeMinutes(Number(e.target.value))}
              required
              className="w-full px-3 py-2 border border-gray-300 rounded-lg"
            />
          </div>
          <button
            type="submit"
            className="w-full bg-amber-600 text-white py-2.5 rounded-lg font-semibold hover:bg-amber-700 transition flex items-center justify-center space-x-1 rtl:space-x-reverse"
          >
            <Plus className="h-5 w-5" />
            <span>{t("create")}</span>
          </button>
        </form>

        <div className="lg:col-span-2">
          {error && <div className="p-4 bg-red-50 text-red-700 rounded-lg mb-4">{error}</div>}
          {loading ? (
            <div className="text-center py-12 text-gray-500">{t("loading")}</div>
          ) : policies.length === 0 ? (
            <div className="bg-white p-12 text-center rounded-xl shadow border border-gray-100 text-gray-500">
              No SLA policies found.
            </div>
          ) : (
            <div className="bg-white shadow rounded-xl overflow-hidden border border-gray-100">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                  <tr>
                    <th className="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase tracking-wider">Name</th>
                    <th className="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase tracking-wider">{t("priority")}</th>
                    <th className="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase tracking-wider">Response (m)</th>
                    <th className="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase tracking-wider">Resolution (m)</th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {policies.map((policy) => (
                    <tr key={policy.id} className="hover:bg-gray-50 transition">
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{policy.name}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm">
                        <span className="px-2.5 py-1 text-xs font-semibold rounded-full bg-amber-50 text-amber-700">
                          {policy.priority}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{policy.responseTimeMinutes}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{policy.resolutionTimeMinutes}</td>
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
